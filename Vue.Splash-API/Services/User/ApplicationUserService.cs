using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Data;
using Vue.Splash_API.Dtos;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Services.User;

public class ApplicationUserService : IApplicationUserService
{
    private readonly SplashContext _context;

    public ApplicationUserService(SplashContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> FindUserById(int id)
    {
        return await _context.ApplicationUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ApplicationUser?> FindUserByIdentifier(string identifier)
    {
        return await FindUserByEmail(identifier) ?? await FindUserByUserName(identifier);
    }

    public async Task<ApplicationUser?> FindUserByUserName(string userName)
    {
        return await _context.ApplicationUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserName == userName);
    }

    public async Task<ApplicationUser?> FindUserByEmail(string email)
    {
        return await _context.ApplicationUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<ApplicationUser> CreateUser(ApplicationUser user, string password)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        var userEntry = _context.ApplicationUsers.Add(user);
        await _context.SaveChangesAsync();
        return userEntry.Entity;
    }

    public async Task UpdateUser(ApplicationUser initialValue, AccountUpdateDto updateDto)
    {
        initialValue.UserName = updateDto.Username;
        initialValue.Email = updateDto.Email;
        _context.ApplicationUsers.Update(initialValue);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePassword(ApplicationUser user, string newPassword)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _context.ApplicationUsers.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(ApplicationUser user)
    {
        _context.ApplicationUsers.Remove(user);
        await _context.SaveChangesAsync();
    }
}