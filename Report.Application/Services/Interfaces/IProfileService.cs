using Report.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Services.Interfaces
{
    public interface IProfileService
    {
        Task<EditProfileViewModel> GetProfileAsync(int userId);
        Task UpdateProfileAsync(EditProfileViewModel model);
    }
}
