using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Models;

namespace PrintStore.Infrastructure.Abstract
{
    /// <summary>
    /// Basic interface for managing users and their roles
    /// </summary>
    public interface IUserLayer
    {
        IEnumerable<ApplicationUser> Users { get; }

        void ChangeUserRole(string userId, string userRole);

        void BlockUser(string userId);

        void UnblockUser(string userId);

        string GetRoleName(string userId);
    }
}
