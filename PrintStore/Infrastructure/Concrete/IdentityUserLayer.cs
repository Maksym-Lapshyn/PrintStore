using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Infrastructure.Abstract;
using PrintStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PrintStore.Infrastructure.Concrete
{
    /// <summary>
    /// User managing functionality embodied with the help of Identity and Entity Framework
    /// </summary>
    public class IdentityUserLayer : IUserLayer
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public IEnumerable<ApplicationUser> Users { get { return context.Users; } }

        /// <summary>
        /// Changes role of a user
        /// </summary>
        /// <remarks>
        /// Only one role is allowed at a time
        /// </remarks>
        /// <param name="userId">Id of user</param>
        /// <param name="userRole">Selected role</param>
        public void ChangeUserRole(string userId, string userRole)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.Users.Where(u => u.Id == userId).First();
            user.Roles.Clear();
            userManager.AddToRole(userId, userRole);
        }

        public void BlockUser(string userId)
        {
            ApplicationUser user = context.Users.Where(u => u.Id == userId).First();
            user.IsBlocked = true;
            context.SaveChanges();
        }

        public void UnblockUser(string userId)
        {
            ApplicationUser user = context.Users.Where(u => u.Id == userId).First();
            user.IsBlocked = false;
            context.SaveChanges();
        }

        /// <summary>
        /// Gets string value for user's role
        /// </summary>
        /// <remarks>
        /// Only one role is allowed at a time
        /// </remarks>
        /// <param name="userId">Id of user to get role for</param>
        /// <returns></returns>
        public string GetRoleName(string userId)
        {
            ApplicationUser user = context.Users.Where(u => u.Id == userId).First();
            string userRoleId = user.Roles.First().RoleId;
            var role = context.Roles.Where(r => r.Id == userRoleId).First();
            return role.Name;
        }
    }
}