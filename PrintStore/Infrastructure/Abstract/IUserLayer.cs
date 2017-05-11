using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStore.Infrastructure.Abstract
{
    public interface IUserLayer
    {
        void ChangeUserRole(string userId, string userRole);

        void BlockUser(string userId);

        void UnblockUser(string userId);
    }
}
