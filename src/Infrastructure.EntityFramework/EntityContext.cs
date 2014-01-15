using System;
using System.Configuration;
using System.Data.Entity;
using Infrastructure.EntityFramework.Mapping.Users;


namespace Infrastructure.EntityFramework
{
    public class EntityContext : DbContext, IEntityContext
    {
        private readonly string _defaultSchema;

        #region CTOR

        public EntityContext(string connectionString) : base(connectionString)
        {
            // Stop EF from changing our database, we'll do it with Fluent Migrator
            Database.SetInitializer<EntityContext>(null);

            // Turn off auto detection of changes for performance increase
            // we're manually setting the entity to modified on updates anyway
            Configuration.AutoDetectChangesEnabled = false;

            // Turn off lazy loading for performance increase, use eager loading - .Include()
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            // Set schema (table namespace) based on web.config (default is "")
            // Example: Portal.Users instead of just Users
            _defaultSchema = ConfigurationManager.AppSettings["DefaultSchema"];
            
            if (_defaultSchema == null)
                throw new ApplicationException("Default schema not set in Web.Config!");
        }

        #endregion

        #region Context Overrides

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Set our schema
            modelBuilder.HasDefaultSchema(_defaultSchema);

            // Apply the mappings
            modelBuilder.Configurations
                .Add(new UserMap());

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
