using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;

namespace SMEAppHouse.Core.Patterns.EF.Helpers;

public static class EntityModelConfigurationExtension
{
    public static PropertyBuilder<object> RegisterConventionalField<TEntity>(this EntityTypeBuilder<TEntity> entityBuilder, Expression<Func<TEntity, object>> selector)
           where TEntity : class, IEntity
    {
        return RegisterConventionalField(entityBuilder, null, selector);
    }

    public static PropertyBuilder<object> RegisterConventionalField<TEntity>(this EntityTypeBuilder<TEntity> entityBuilder
        , IList<string> fieldsIgnoreList
        , Expression<Func<TEntity, object>> selector) where TEntity : class, IEntity
    {
        var fNm = GetFieldNameFromSelector(selector);
        if (fieldsIgnoreList != null && fieldsIgnoreList.Contains(fNm))
            return null;
        else 
            return entityBuilder.Property(selector);
    }

    #region IEntityTypeConfiguration Extensions

    /// <summary>
    /// https://stackoverflow.com/a/12420603
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="fieldSelector"></param>
    /// <returns></returns>
    internal static string GetFieldNameFromSelector<TEntity>(Expression<Func<TEntity, object>> fieldSelector)
        where TEntity : class, IEntity
    {
        if (fieldSelector.Body is MemberExpression expression)
            return expression.Member.Name;
        else
        {
            var op = ((UnaryExpression)fieldSelector.Body).Operand;
            return ((MemberExpression)op).Member.Name;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="conventionFieldsToIgnore"></param>
    /// <returns></returns>
    internal static IList<string> ToListOfFields<TEntity>(this Expression<Func<TEntity, object>>[] conventionFieldsToIgnore)
        where TEntity : class, IEntity
    {
        var includes = new List<string>();
        if (conventionFieldsToIgnore == null || !conventionFieldsToIgnore.Any()) return includes;
        includes.AddRange(conventionFieldsToIgnore.Select(GetFieldNameFromSelector));
        return includes;
    }

    internal static void IgnoreConventionFields<TEntity>(this ModelBuilder modelBuilder,
        Expression<Func<TEntity, object>>[] conventionFieldsToIgnore)
        where TEntity : class, IEntity
    {
        var includes = conventionFieldsToIgnore.ToListOfFields();
        IgnoreConventionFields<TEntity>(modelBuilder, includes);
    }

    internal static void IgnoreConventionFields<TEntity>(this ModelBuilder modelBuilder, IList<string> ignoreLIst)
        where TEntity : class, IEntity
    {

        /*

        Expression<Func<TEntity, object>> selector = p => p.Ordinal;
        fNm = GetFieldNameFromSelector(selector);
        if (includes.Contains(fNm))
            builder.Entity<T>().Ignore(p => p.Ordinal);
        else builder.Entity<T>()
                .Property(p => p.Ordinal)
                .HasColumnName("ordinal")
                .HasDefaultValue(0);

        selector = p => p.IsActive;
        fNm = GetFieldNameFromSelector(selector);
        if (includes.Contains(fNm))
            builder.Entity<T>().Ignore(p => p.IsActive);
        else builder.Entity<T>()
            .Property(p => p.Ordinal)
            .HasColumnName("ordinal")
            .HasDefaultValue(0);

        selector = p => p.CreatedBy;
        fNm = GetFieldNameFromSelector(selector);
        if (includes.Contains(fNm))
            builder.Entity<T>().Ignore(p => p.CreatedBy);
        else builder.Entity<T>()
            .Property(p => p.Ordinal)
            .HasColumnName("ordinal")
            .HasDefaultValue(0);

        selector = p => p.RevisedBy;
        fNm = GetFieldNameFromSelector(selector);
        if (includes.Contains(fNm))
            builder.Entity<T>().Ignore(p => p.RevisedBy);
        else builder.Entity<T>()
            .Property(p => p.Ordinal)
            .HasColumnName("ordinal")
            .HasDefaultValue(0);

        selector = p => p.DateCreated;
        fNm = GetFieldNameFromSelector(selector);
        if (includes.Contains(fNm))
            builder.Entity<T>().Ignore(p => p.DateCreated);
        else builder.Entity<T>()
            .Property(p => p.Ordinal)
            .HasColumnName("ordinal")
            .HasDefaultValue(0);

        selector = p => p.DateRevised;
        fNm = GetFieldNameFromSelector(selector);
        if (includes.Contains(fNm))
            builder.Entity<T>().Ignore(p => p.DateRevised);
        else builder.Entity<T>()
            .Property(p => p.Ordinal)
            .HasColumnName("ordinal")
            .HasDefaultValue(0);

        */
    }

    #endregion

    #region IEntity Config Extensions

    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object>> fieldSelector,
        bool isRequired,
        string propertyType,
        Action<PropertyBuilder<object>> propertyBuilderAction)

        where T : class, IEntity =>
                builder.DefineDbField(fieldSelector, isRequired, 0, "", propertyType, propertyBuilderAction);

    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object>> fieldSelector,
        bool isRequired,
        string propertyType)
        where T : class, IEntity =>
                builder.DefineDbField(fieldSelector, isRequired, 0, "", propertyType, null);

    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object>> fieldSelector)
        where T : class, IEntity//IGenericEntityBase<int>
                => builder.DefineDbField(fieldSelector, false, 0, "", "", null);

    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object>> fieldSelector,
        bool isRequired)
        where T : class, IEntity
        => builder.DefineDbField(fieldSelector, isRequired, 0, "", "", null);


    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object>> fieldSelector,
        bool isRequired,
        Action<PropertyBuilder<object>> propertyBuilderAction)
        where T : class, IEntity
        => builder.DefineDbField(fieldSelector, isRequired, 0, "", "", propertyBuilderAction);

    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object>> fieldSelector,
        bool isRequired,
        int maxLength)
        where T : class, IEntity
        => builder.DefineDbField(fieldSelector, isRequired, maxLength, "", "", null);

    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
        Expression<Func<T, object>> fieldSelector,
        int maxLength)
        where T : class, IEntity
        => builder.DefineDbField(fieldSelector, false, maxLength, "", "", null);

    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
                                                            Expression<Func<T, object>> fieldSelector,
                                                            bool isRequired,
                                                            int maxLength,
                                                            string fieldName,
                                                            string propertyType)
        where T : class, IEntity
        => builder.DefineDbField(fieldSelector, isRequired, maxLength, fieldName, propertyType, null);


    public static EntityTypeBuilder<T> DefineDbField<T>(this EntityTypeBuilder<T> builder,
                                                            Expression<Func<T, object>> fieldSelector,
                                                            bool isRequired,
                                                            int maxLength,
                                                            string fieldName,
                                                            string propertyType,
                                                            Action<PropertyBuilder<object>> propertyBuilderAction)
        //out PropertyBuilder<object> propBldr)
        where T : class, IEntity // IGenericEntityBase<int>
    {
        var pB = builder.Property(fieldSelector);
        var memberName = GetFieldNameFromSelector(fieldSelector);

        pB.HasColumnName(string.IsNullOrEmpty(fieldName) ? memberName : fieldName);
        if (maxLength > 0)
            pB.HasMaxLength(maxLength);

        if (!string.IsNullOrEmpty(propertyType))
            pB.HasColumnType(propertyType);

        if (!isRequired)
            pB.IsRequired(false);

        propertyBuilderAction?.Invoke(pB);

        //propBldr = pB;
        return builder;
    }


    #endregion
}