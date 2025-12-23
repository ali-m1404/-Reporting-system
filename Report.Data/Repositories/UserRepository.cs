using Data.Context;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Report.Application.Services.Interfaces;
using Report.Domain.Contracts;



namespace Report.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ReportingSystemDbContext _context;
       

        public UserRepository(ReportingSystemDbContext context, IFileService fileService)
        {
            _context = context;
            
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var normalizedEmail = email?.Trim().ToLower();

            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            // نرمال‌سازی ایمیل به حروف کوچک
            var normalizedEmail = email?.Trim().ToLower();

            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == normalizedEmail);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneNumber)
        {
            return await _context.Users
                .AnyAsync(u => u.PhoneNumber == phoneNumber);
        }



        //public void Add(RegisterUserViewModel user)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Delete(User user)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<User> GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public User? GetById(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

    

        //public void Update(User user)
        //{
        //    throw new NotImplementedException();
        //}

        //List<T> IUserRepository.GetAll()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
