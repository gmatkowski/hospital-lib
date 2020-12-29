namespace Hospital.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Custom Entity Framework Data Annotation to support and enforce Uniqueness contraints at the Context level.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class UniqueAttribute : Attribute
    {
        // Private fields.
        private readonly string _name;
        private readonly string _label;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueAttribute"/> class.
        /// </summary>
        /// <param name="label">The label of the constraint. Properties or fields that use the same label with be composited into the same unique constraint.</param>
        /// <param name="name">The column name of the field. If unset, the name is assumed to be the name of the property/field.</param>
        public UniqueAttribute(string label = null, [CallerMemberName]string name = null)
        {
            _name = name; // This is the name of the column in the data table
            _label = (String.IsNullOrWhiteSpace(label)) ? name : label; // The label is used to create composite unique constraints
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the label of the contraint.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public virtual string Label
        {
            get { return _label; }
        }
    }

    public static class UniqueAttributeDbContextExtensions
    {
        public static DbValidationError ValidateUniqueAttributes(this DbContext context, DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified)
            {
                // Can Validate for Unique values here, have access to the 
                var uniqueProperties = new List<Tuple<string, string>>();
                var type = entityEntry.Entity.GetType();
                var entityName = (type.BaseType != null) ? type.BaseType.Name : type.Name;
                var props = type.GetProperties();

                // Get all Unique Attributes for the Entity
                foreach (var prop in props)
                {
                    var attrs = prop.GetCustomAttributes(true);
                    foreach (var attr in attrs)
                    {
                        var uniqueAttr = attr as UniqueAttribute;
                        if (uniqueAttr == null) continue;

                        uniqueProperties.Add(new Tuple<string, string>(uniqueAttr.Label, uniqueAttr.Name));
                    }
                }

                // Convert to Lookup table, and then check for uniqueness on fields
                foreach (var group in uniqueProperties.ToLookup(x => x.Item1, x => x.Item2))
                {
                    var label = group.Key;
                    var query = context.Set(type).AsQueryable();

                    var originalValues = (entityEntry.State == EntityState.Added) ? new Dictionary<string, object>() : group.ToDictionary(name => name, name => entityEntry.OriginalValues[name]);
                    var currentValues = group.ToDictionary(name => name, name => entityEntry.CurrentValues[name]);

                    // According to the ANSI standards SQL:92, SQL:1999, and SQL:2003, a UNIQUE constraint should disallow duplicate non-NULL values, but allow multiple NULL values.
                    if (currentValues.Any(column => column.Value == null))
                    {
                        continue;
                    }

                    query = originalValues.Aggregate(query, (current, column) => current.WhereEquals(column.Key, column.Value, false));
                    query = currentValues.Aggregate(query, (current, column) => current.WhereEquals(column.Key, column.Value));

                    if (query.Any())
                    {
                        return new DbValidationError(label,
                            string.Format("Taka nazwa jest już zajęta", entityName, String.Join(" and ", group.ToArray())));
                    }
                }
            }

            return null;
        }

        private static IQueryable WhereEquals(this IQueryable query, string name, object value, bool equals = true)
        {
            // Dynamic Linq should convert the relevant predicate into compatible syntax for whatever database is being used.
            const string valueTypeEqualsPredicate = "{0} = @0";
            const string valueTypeNotEqualsPredicate = "{0} != @0";
            const string structEqualsPredicate = "{0}.Equals(@0)";
            const string structNotEqualsPredicate = "!{0}.Equals(@0)";

            // The default predicate should be fine for the integral types.
            var predicateString = (equals) ? valueTypeEqualsPredicate : valueTypeNotEqualsPredicate;
            var valueType = value.GetType();

            // Structs should use the Equals method to compare with. This should satisfy Guids, DateTimes
            // http://stackoverflow.com/questions/1827425/how-to-check-programatically-if-a-type-is-a-struct-or-a-class
            if (valueType.IsValueType && !valueType.IsPrimitive && !valueType.IsEnum)
            {
                predicateString = (equals) ? structEqualsPredicate : structNotEqualsPredicate;
            }

            return query.Where(String.Format(predicateString, name), value);
        }
    }
}