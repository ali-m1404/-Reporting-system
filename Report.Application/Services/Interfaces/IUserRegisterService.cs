using Domain.Entities.Users;
using Report.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Services.Interfaces
{
    public interface IUserRegisterService
    {
        User ValidateUser(string email, string password);
        Task <bool> Create(RegisterUserViewModel model, string email, string phoneNumber);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber);
       

    }
}   
    
