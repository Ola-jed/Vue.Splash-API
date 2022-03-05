using System.Threading.Tasks;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.User;

public interface IApplicationUserService
{
    Task<ApplicationUser?> FindUserById(int id);
    Task<ApplicationUser?> FindUserByIdentifier(string identifier);
    Task<ApplicationUser?> FindUserByUserName(string userName);
    Task<ApplicationUser?> FindUserByEmail(string email);
    Task<ApplicationUser> CreateUser(ApplicationUser user, string password);
    Task UpdateUser(ApplicationUser initialValue, AccountUpdateDto updateDto);
    Task UpdatePassword(ApplicationUser user, string newPassword);
    Task DeleteUser(ApplicationUser user);
}