using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Models;

namespace PrintStore.Infrastructure.Abstract
{
    public interface IUserLayer
    {
        IEnumerable<ApplicationUser> Users { get; }

        void ChangeUserRole(string userId, string userRole);

        void BlockUser(string userId);

        void UnblockUser(string userId);

        string GetRoleName(string userId);
    }
}
