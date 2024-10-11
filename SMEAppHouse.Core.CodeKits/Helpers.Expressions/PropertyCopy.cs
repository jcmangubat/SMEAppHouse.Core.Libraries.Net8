using System.Linq.Expressions;
using System.Reflection;

namespace SMEAppHouse.Core.CodeKits.Helpers.Expressions
{
    /// <summary>
    /// http://stackoverflow.com/questions/930433/apply-properties-values-from-one-object-to-another-of-the-same-type-automaticall
    /// Generic class which copies to its target type from a source
    /// type specified in the Copy method. The types are specified
    /// separately to take advantage of type inference on generic
    /// method arguments.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public static class PropertyCopy<TTarget> where TTarget : class, new()
    {
        /// <summary>
        /// Copies all readable properties from the source to a new instance
        /// of TTarget.
        /// </summary>
        public static TTarget CopyFrom<TSource>(TSource source) where TSource : class
        {
            return PropertyCopier<TSource, TTarget>.Copy(source);
        }

        /*/// <summary>
        /// Copies all public, readable properties from the source object to the
        /// target. The target type does not have to have a parameterless constructor,
        /// as no new instance needs to be created.
        /// </summary>
        /// <remarks>Only the properties of the source and target types themselves
        /// are taken into account, regardless of the actual types of the arguments.</remarks>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <typeparam name="TTarget">Type of the target</typeparam>
        /// <param name="source">Source to copy properties from</param>
        /// <param name="target">Target to copy properties to</param>
        public static void Copy<TSource, TTarget>(TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            PropertyCopier<TSource, TTarget>.Copy(source, target);
        }*/
    }

    public static class PropertyCopier<TSource, TTarget>
    {
        /// <summary>
        /// Delegate to create a new instance of the target type given an instance of the
        /// source type. This is a single delegate from an expression tree.
        /// </summary>
        private static readonly Func<TSource, TTarget> Creator;

        /*/// <summary>
        /// List of properties to grab values from. The corresponding targetProperties 
        /// list contains the same properties in the target type. Unfortunately we can't
        /// use expression trees to do this, because we basically need a sequence of statements.
        /// We could build a DynamicMethod, but that's significantly more work :) Please mail
        /// me if you really need this...
        /// </summary>*/
        private static readonly List<PropertyInfo> SourceProperties = new();
        private static readonly List<PropertyInfo> TargetProperties = new();
        private static readonly Exception InitializationException;

        internal static TTarget Copy(TSource source)
        {
            if (InitializationException != null)
                throw InitializationException;

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Creator(source);
        }

        internal static void Copy(TSource source, TTarget target)
        {
            if (InitializationException != null)
            {
                throw InitializationException;
            }
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            for (var i = 0; i < SourceProperties.Count; i++)
            {
                TargetProperties[i].SetValue(target, SourceProperties[i].GetValue(source, null), null);
            }

        }

        static PropertyCopier()
        {
            try
            {
                Creator = BuildCreator();
                InitializationException = null;
            }
            catch (Exception e)
            {
                Creator = null;
                InitializationException = e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Func<TSource, TTarget> BuildCreator()
        {
            var sourceParameter = Expression.Parameter(typeof(TSource), "source");
            var bindings = new List<MemberBinding>();
            foreach (var sourceProperty in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!sourceProperty.CanRead)
                    continue;

                var targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);

                if (targetProperty == null)

                    throw new ArgumentException("Property " + sourceProperty.Name + " is not present and accessible in " + typeof(TTarget).FullName);

                if (!targetProperty.CanWrite)
                    throw new ArgumentException("Property " + sourceProperty.Name + " is not writable in " + typeof(TTarget).FullName);

                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                    throw new ArgumentException("Property " + sourceProperty.Name + " is static in " + typeof(TTarget).FullName);

                if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    throw new ArgumentException("Property " + sourceProperty.Name + " has an incompatible type in " + typeof(TTarget).FullName);

                bindings.Add(Expression.Bind(targetProperty, Expression.Property(sourceParameter, sourceProperty)));
                SourceProperties.Add(sourceProperty);
                TargetProperties.Add(targetProperty);
            }
            Expression initializer = Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);
            return Expression.Lambda<Func<TSource, TTarget>>(initializer, sourceParameter).Compile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumDictionary<T>()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T is not an Enum type");

            if (Enum.GetUnderlyingType(typeof(T)) != typeof(int))
                throw new ArgumentException("The underlying type of the enum T is not Int32");

            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToDictionary(t => (int)(object)t, t => t.ToString());
        }

    }

    //public class PropertyCopier<TSource, TTarget>
    //    where TSource : class
    //    where TTarget : class
    //{

    //    private readonly bool _ignoreNullValues = false;

    //    /// <summary>
    //    /// Delegate to create a new instance of the target type given an instance of the
    //    /// source type. This is a single delegate from an expression tree.
    //    /// </summary>
    //    private readonly Func<TSource, TTarget> Creator;

    //    /*/// <summary>
    //    /// List of properties to grab values from. The corresponding targetProperties 
    //    /// list contains the same properties in the target type. Unfortunately we can't
    //    /// use expression trees to do this, because we basically need a sequence of statements.
    //    /// We could build a DynamicMethod, but that's significantly more work :) Please mail
    //    /// me if you really need this...
    //    /// </summary>*/
    //    private readonly List<PropertyInfo> SourceProperties = new List<PropertyInfo>();
    //    private readonly List<PropertyInfo> TargetProperties = new List<PropertyInfo>();
    //    private readonly Exception InitializationException;

    //    #region constructors

    //    public PropertyCopier() : this(false)
    //    {
    //    }

    //    public PropertyCopier(bool ignoreInaccessibles = false) :
    //        this(ignoreInaccessibles, false)
    //    {
    //    }

    //    public PropertyCopier(bool ignoreInaccessibles = false, bool ignoreNullValues = false)
    //    {
    //        try
    //        {
    //            _ignoreNullValues = ignoreNullValues;
    //            Creator = BuildCreator(ignoreInaccessibles);
    //            InitializationException = null;
    //        }
    //        catch (Exception e)
    //        {
    //            Creator = null;
    //            InitializationException = e;
    //        }
    //    }

    //    #endregion

    //    internal TTarget Copy(TSource source)
    //    {
    //        if (InitializationException != null)
    //            throw InitializationException;

    //        if (source == null)
    //            throw new ArgumentNullException(nameof(source));

    //        return Creator(source);
    //    }

    //    internal void Copy(TSource source, TTarget target, bool? ignoreNullValues = false)
    //    {
    //        if (InitializationException != null)
    //        {
    //            throw InitializationException;
    //        }
    //        if (source == null)
    //        {
    //            throw new ArgumentNullException(nameof(source));
    //        }
    //        for (var i = 0; i < SourceProperties.Count; i++)
    //        {
    //            var value = SourceProperties[i].GetValue(source, null);
    //            var ignoreNullVal = (ignoreNullValues.HasValue && ignoreNullValues.Value) || _ignoreNullValues;

    //            var ignorable = value == null;
    //            if (SourceProperties[i].PropertyType.Name.Equals("Guid"))
    //                ignorable = value == null || value.ToString().Equals(Guid.Empty.ToString());

    //            if (i == 96)
    //            {
    //            }

    //            if (!ignorable && !ignoreNullVal)
    //                TargetProperties[i].SetValue(target, value, null);
    //        }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    private Func<TSource, TTarget> BuildCreator(bool ignoreInaccessibles = false)
    //    {
    //        var sourceParameter = Expression.Parameter(typeof(TSource), "source");
    //        var bindings = new List<MemberBinding>();
    //        foreach (var sourceProperty in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance))
    //        {
    //            if (!sourceProperty.CanRead)
    //                continue;

    //            var targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);

    //            if (targetProperty == null)
    //                if (ignoreInaccessibles) continue;
    //                else throw new ArgumentException("Property " + sourceProperty.Name + " is not present and accessible in " + typeof(TTarget).FullName);
    //            else
    //            {
    //                if (!targetProperty.CanWrite)
    //                    throw new ArgumentException("Property " + sourceProperty.Name + " is not writable in " + typeof(TTarget).FullName);

    //                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
    //                    throw new ArgumentException("Property " + sourceProperty.Name + " is static in " + typeof(TTarget).FullName);

    //                if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
    //                    throw new ArgumentException("Property " + sourceProperty.Name + " has an incompatible type in " + typeof(TTarget).FullName);

    //                bindings.Add(Expression.Bind(targetProperty, Expression.Property(sourceParameter, sourceProperty)));
    //                TargetProperties.Add(targetProperty);
    //            }

    //            SourceProperties.Add(sourceProperty);
    //        }
    //        Expression initializer = Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);
    //        return Expression.Lambda<Func<TSource, TTarget>>(initializer, sourceParameter).Compile();
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <returns></returns>
    //    public Dictionary<int, string> GetEnumDictionary<T>()
    //    {
    //        if (!typeof(T).IsEnum)
    //            throw new ArgumentException("T is not an Enum type");

    //        if (Enum.GetUnderlyingType(typeof(T)) != typeof(int))
    //            throw new ArgumentException("The underlying type of the enum T is not Int32");

    //        return Enum.GetValues(typeof(T))
    //            .Cast<T>()
    //            .ToDictionary(t => (int)(object)t, t => t.ToString());
    //    }
    //}
}