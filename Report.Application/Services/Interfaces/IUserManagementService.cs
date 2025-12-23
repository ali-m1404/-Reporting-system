using Domain.Entities.Users;
using Report.Application.Dtos;
using Report.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Services.Contracts
{
    public interface IUserManagementService
    {
        Task<List<UserDto>> GetUsersAsync(string serch,string role,string status);
        Task DisableUserAsync(int userId, string performedBy);
        Task EnableUserAsync(int userId);
        Task ChangeUserRoleAsync(int userId, int newRole, string performedBy);
        Task ResetPasswordAsync(int userId);


        Task<EditUserViewModel?> GetUserForEditAsync(int userId);

        Task UpdateUserAsync(EditUserViewModel model);

        Task DeleteAsync(int id);


    }
}
