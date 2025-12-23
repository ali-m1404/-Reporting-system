using Domain.Entities;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public interface IUserRepository2
    {
        Task<List<User>> GetUsersAsync(string search,string role, string status);
        Task<User?> GetByIdAsync(int userId);
        Task<bool> IsLastAdminAsync(int  userId);
        Task UpdateAsync(User user);
        Task<int> CountAdminsAsync();
        Task<bool> ExistsUserNameAsync(string userName);
        Task<bool> ExistsEmailAsync(string email);
        Task SaveAsync();
        Task DeleteAsync(User user);

    }
}
