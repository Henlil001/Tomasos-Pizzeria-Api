using Tomasos_Pizzeria.Domain.DTO;

namespace Tomasos_Pizzeria.Core.Interfaces
{
    public interface IApplicationUserService
    {
        Task<string> UserLoginAsync(string username, string password);
        Task<bool> AddUserAsync(UserDTO user);
        Task<bool> UpgradeToPremiumAsync(string username);
        Task<bool> DownGradeToRegularAsync(string username);
        Task<List<ApplicationUserWithRolesDTO>> GetAllUsersWithRoles();
        Task<bool> UpdateUserAsync(UserDTO user, string id);
        Task<bool> ChangePasswordAsync(string newPassword, string userId);

    }
}
