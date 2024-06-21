using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

namespace SMEAppHouse.Core.Patterns.EF.Helpers;

public static class Utilities
{
    public static void UpdateValuesFrom<TEntityDest, TEntitySource>(this TEntityDest destinationObject, TEntitySource sourceObject)
        where TEntitySource : class
        where TEntityDest : class
    {
        if (destinationObject == null || sourceObject == null)
            throw new ArgumentNullException("Objects cannot be null");

        var sourceProperties = typeof(TEntitySource).GetProperties();
        var destinationProperties = typeof(TEntityDest).GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);

            if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
            {
                var sourceValue = sourceProperty.GetValue(sourceObject);

                // Check if the property is nullable and the source value is null
                if (Nullable.GetUnderlyingType(destinationProperty.PropertyType) != null && sourceValue == null)
                    continue; // Skip updating the destination property

                destinationProperty.SetValue(destinationObject, sourceValue);
            }
        }
    }

    /// <summary>
    /// Read a SQL script that is embedded as a resource named after the migration class.
    /// </summary>
    /// <param name="TMigrationSQLScriptType">The migration type the SQL file script is attached to.</param>
    /// <param name="upOrDownScaleFilePrefix">Optional parameter providing a strategy to distinguish between UP or DOWN SQL scripts.</param>
    /// <returns>The content of the SQL file.</returns>
    public static string ReadSql<MigrationType>(this MigrationType migrationType, string upOrDownScaleFilePrefix = "")
        where MigrationType : Migration
    {
        var type = typeof(MigrationType);
        var customAttribute = type.CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(MigrationAttribute));
        var migrationAttributeName = customAttribute.ConstructorArguments[0].Value.ToString();

        if (!string.IsNullOrEmpty(upOrDownScaleFilePrefix))
        {
            migrationAttributeName += upOrDownScaleFilePrefix;
        }

        var assembly = Assembly.GetExecutingAssembly();
        var sqlFile = assembly.GetManifestResourceNames().FirstOrDefault(scriptFile => scriptFile.Contains(migrationAttributeName));
        if (string.IsNullOrEmpty(sqlFile))
            return string.Empty;

        using var stream = assembly.GetManifestResourceStream(sqlFile);
        using StreamReader reader = new(stream);
        var sqlScript = reader.ReadToEnd();

        return sqlScript;
    }

    /*public static Expression<Func<TModel, bool>> BuildPredicate<TModel>(this Expression<Func<TModel, bool>> filter)
    {
        // Create a parameter expression for the target entity type (TModel)
        ParameterExpression parameter = Expression.Parameter(typeof(TModel), "x");

        // Replace the parameter in the original predicate with the parameter for the target entity type
        Expression body = new ParameterExpressionVisitor(parameter).Visit(filter.Body);

        // Create a new lambda expression with the modified body
        Expression<Func<TModel, bool>> predicate = Expression.Lambda<Func<TModel, bool>>(body, parameter);

        return predicate;
    }*/


    /*public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var body = Expression.AndAlso(left.Body, right.Body);
        return Expression.Lambda<Func<T, bool>>(body, left.Parameters);
    }*/

    /*if (filter == null)
        throw new ArgumentNullException(nameof(filter));

    if (propertyExpression == null)
        throw new ArgumentNullException(nameof(propertyExpression));*/

    /*public static bool IsPropertySetInFilter<T>(Expression<Func<T, bool>> filter, Expression<Func<T, object>> propertyExpression)
    {
        if (filter == null)
            throw new ArgumentNullException(nameof(filter));

        if (propertyExpression == null)
            throw new ArgumentNullException(nameof(propertyExpression));

        MemberExpression memberExpression = null;

        if (propertyExpression.Body.NodeType == ExpressionType.Convert)
        {
            memberExpression = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
        }
        else if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = propertyExpression.Body as MemberExpression;
        }

        if (memberExpression == null)
        {
            throw new ArgumentException("Invalid property expression");
        }

        string propertyName = memberExpression.Member.Name;

        var conditions = filter.Body.GetConditions();

        foreach (var condition in conditions)
        {
            if (condition.NodeType == ExpressionType.Equal)
            {
                var binaryExpression = (BinaryExpression)condition;
                var foundIt = false;

                if (binaryExpression.Left.NodeType == ExpressionType.Convert)
                {
                    // Handle Convert node type
                    var convertExpression = (UnaryExpression)binaryExpression.Left;
                    if (convertExpression.Operand.NodeType == ExpressionType.MemberAccess)
                    {
                        var leftMemberExpression = (MemberExpression)convertExpression.Operand;
                        foundIt = leftMemberExpression.Member.Name == propertyName;
                    }
                }
                else if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
                {
                    var leftMemberExpression = (MemberExpression)binaryExpression.Left;
                    foundIt = leftMemberExpression.Member.Name == propertyName;
                }

                if (foundIt)
                    return true;
            }
        }

        return false;
    }*/

    /*private static bool CheckFilterForProperty(Expression expression, string propertyName)
    {
        if (expression is BinaryExpression binaryExpression)
        {
            return CheckBinaryExpressionForProperty(binaryExpression, propertyName);
        }
        else if (expression is MethodCallExpression methodCallExpression)
        {
            return methodCallExpression.Arguments.Any(arg => CheckFilterForProperty(arg, propertyName));
        }

        return false;
    }*/

    /*private static bool CheckBinaryExpressionForProperty(BinaryExpression binaryExpression, string propertyName)
    {
        bool leftCondition = false;
        bool rightCondition = false;

        foreach (var condition in binaryExpression.GetConditions())
        {
            if (condition.NodeType == ExpressionType.Convert)
            {
                // Handle Convert node type
                var convertExpression = (UnaryExpression)condition;
                if (convertExpression.Operand.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)convertExpression.Operand;
                    leftCondition = memberExpression.Member.Name == propertyName;
                }
            }
            else if (condition.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = (MemberExpression)condition;
                leftCondition = memberExpression.Member.Name == propertyName;
            }

            // Check if the condition is a comparison with a constant value
            rightCondition = condition.NodeType == ExpressionType.Constant;

            // Break the loop if any condition is true
            if (leftCondition || rightCondition)
            {
                break;
            }
        }

        return leftCondition && rightCondition;
    }*/

    /* public static IEnumerable<Expression> GetConditions(this Expression expression)
     {
         var conditions = new List<Expression>();

         // Recursive method to traverse the expression tree
         void Visit(Expression node)
         {
             if (node.NodeType == ExpressionType.Convert)
             {
                 var convertExpression = (UnaryExpression)node;
                 Visit(convertExpression.Operand);
             }
             else if (node.NodeType == ExpressionType.AndAlso || node.NodeType == ExpressionType.OrElse)
             {
                 var binaryExpression = (BinaryExpression)node;
                 Visit(binaryExpression.Left);
                 Visit(binaryExpression.Right);
             }
             else
             {
                 conditions.Add(node);
             }
         }

         Visit(expression);

         return conditions;
     }*/
}
