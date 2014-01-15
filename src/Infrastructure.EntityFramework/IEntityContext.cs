using System.Data.Entity;
using Core.Domain.Model.Users;

namespace Infrastructure.EntityFramework
{
    public interface IEntityContext
    {
        DbSet<User> Users { get; set; }
    }
}
