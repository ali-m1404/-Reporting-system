using Domain.Entities.Users;
using Report.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.Contracts
{
    public interface IUserRepository
    {
        //List<User> GetAll();

        void Add(User user);
        User GetUserByEmail(string email);

        Task SaveAsync(); 
        Task<bool> IsEmailExistAsync(string email);
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);

        //User? GetById(int id);

        //void Update(User user);
        //void Delete(User user);

    }
}
