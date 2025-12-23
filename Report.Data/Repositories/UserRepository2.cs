using Data.Context;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Report.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Repositories
{
    
        public class UserRepository2 : IUserRepository2
        {
            private readonly ReportingSystemDbContext _context;


            public UserRepository2(ReportingSystemDbContext context)
            {
                _context = context;
            }


        public async Task<List<User>> GetUsersAsync(string search, string role, string status)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search));

            if (!string.IsNullOrWhiteSpace(role))
            {
                switch (role.ToLower())
                {
                    case "superadmin":
                        query = query.Where(u => u.RoleId == 1);
                        break;
                    case "admin":
                        query = query.Where(u => u.RoleId == 2);
                        break;
                    case "user":
                        query = query.Where(u => u.RoleId == 3);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                switch (status.ToLower())
                {
                    case "active":
                        query = query.Where(u => u.IsActive);
                        break;
                    case "inactive":
                        query = query.Where(u => !u.IsActive);
                        break;
                    case "pending":
                        query = query.Where(u => u.IsActive == null);
                        break;
                }
            }

            return await query.ToListAsync();
        }



        public async Task<User?> GetByIdAsync(int userId)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            }


            public async Task<bool> IsLastAdminAsync(int userId)
            {
                var adminCount = await _context.Users.CountAsync(u => u.RoleId == 2 && u.IsActive);
                var user = await GetByIdAsync(userId);
                return adminCount == 1 && user?.RoleId == 2;
            }


            public async Task UpdateAsync(User user)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

        public async Task<int> CountAdminsAsync()
        {
            return await _context.Users
                .CountAsync(u => u.RoleId == 2 && u.IsActive);
        }

        // ذخیره تغییرات
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsUserNameAsync(string userName)
        {
            return await _context.Users.AnyAsync(u => u.FirstName == userName);
        }

        // ✅ پیاده‌سازی ExistsEmailAsync
        public async Task<bool> ExistsEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await Task.CompletedTask; // فقط برای یکدست بودن async
        }

    }

}
