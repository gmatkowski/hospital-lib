using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq.Dynamic;
using Hospital.Abstracts;
using Hospital.Models;
using Hospital.Rules;

namespace Hospital.DB
{
    public class HospitalContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Shift> Shifts { get; set; }

        public HospitalContext(string cs) : base(cs) { }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var errors = new List<DbValidationError>();
            var error = this.ValidateUniqueAttributes(entityEntry, items);
            if (error != null)
            {
                errors.Add(error);
            }

            if (errors.Any())
            {
                return new DbEntityValidationResult(entityEntry, errors);
            }

            return base.ValidateEntity(entityEntry, items);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

    }
}
