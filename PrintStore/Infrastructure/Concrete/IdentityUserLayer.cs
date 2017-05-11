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
    public class IdentityUserLayer : IUserLayer
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public IEnumerable<ApplicationUser> Users { get { return context.Users; } }

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

        public string GetRoleName(string userId)
        {
            ApplicationUser user = context.Users.Where(u => u.Id == userId).First();
            string userRoleId = user.Roles.First().RoleId;
            var role = context.Roles.Where(r => r.Id == userRoleId).First();
            return role.Name;
        }
    }
}