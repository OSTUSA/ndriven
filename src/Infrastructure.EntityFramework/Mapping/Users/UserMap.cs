using System.Data.Entity.ModelConfiguration;
using Core.Domain.Model.Users;

namespace Infrastructure.EntityFramework.Mapping.Users
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("Users");

            HasKey(x => x.Id);

            //===========================================
            // EXAMPLES
            //===========================================

            // Many to one navigation property
            //HasMany(x => x.Ingredients)
            //    .WithRequired(y => y.Formula)
            //    .HasForeignKey(y => y.FormulaId);

            // Set precision of decimals, or EF will truncate at two decimals
            //Property(x => x.SpecificGravity).HasPrecision(16, 6);
            
            // If there are helper properties on the model, ignore them here
            //Ignore(x => x.MeasureUnits);
        }
    }
}
