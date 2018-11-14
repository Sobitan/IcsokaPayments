using System.Collections.Generic;
using System.Data.Entity;
using IcsokaPayments.Data.Infrastructure;
using IcsokaPayments.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IcsokaPayments.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public void AssignRole(string userName, List<string> roleNames)
        {
            var user = this.GetById(userName);
            if (user == null)
            {
                return;
            }
            user.Roles.Clear();
            foreach (var roleName in roleNames)
            {
                var role = this.DataContext.Roles.Find(roleName);
                if (role == null) continue;
                var newRole = new IdentityUserRole() { RoleId = role.Id };

                user.Roles.Add(newRole);
            }

            this.DataContext.Users.Attach(user);
            this.DataContext.Entry(user).State = EntityState.Modified;
        }



        public void AssignRole(string userName, string roleName)
        {
            var user = this.GetById(userName);
            if (user == null)
            {
                return;
            }
        }
    }

    public interface IUserRepository : IRepository<User>
    {
        void AssignRole(string userName, List<string> roleName);
        void AssignRole(string userName, string roleName);
    }
}
