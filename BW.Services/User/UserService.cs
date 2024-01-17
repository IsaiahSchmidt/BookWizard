using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using BW.Data;
using BW.Data.Entities;
using BW.Models.User;

namespace BW.Services.User;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;

    public UserService(ApplicationDbContext context,
                        UserManager<UserEntity> userManager,
                        SignInManager<UserEntity> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<bool> RegisterUserAsync(UserRegister model)
    {
        if (await CheckEmailAvailability(model.Email) == false)
        {
            System.Console.WriteLine("Email is already in use");
            return false;
        }
        if (await CheckEmailAvailability(model.UserName) == false)
        {
            System.Console.WriteLine("Username is already in use");
            return false;
        }
        UserEntity entity = new()
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName
        };

        IdentityResult registerResult = await _userManager.CreateAsync(entity, model.Password);
        return registerResult.Succeeded;
    }

    public async Task<UserDetail?> GetUserByIdAsync(int userId)
    {
        var entity = await _context.Users.FindAsync(userId);
        if (entity is null)
        {
            return null;
        }

        UserDetail detail = new()
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            UserName = entity.UserName
        };
        return detail;
    }

    private async Task<bool> CheckEmailAvailability(string email)
    {
        UserEntity? existingUser = await _userManager.FindByEmailAsync(email);
        return existingUser is null;
    }

    public async Task<bool> DeleteUserAsync(int userId) {
        var userEntity = await _context.Users.FindAsync(userId);

          if(userEntity == null) 
            return false;

        _context.Users.Remove(userEntity);
        return await _context.SaveChangesAsync() == 1;
    }
}