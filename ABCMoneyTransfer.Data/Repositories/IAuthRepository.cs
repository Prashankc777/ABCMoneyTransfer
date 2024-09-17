using ABCMoneyTransfer.Data.AuthModels;
using ABCMoneyTransfer.Data.Entities;
using ABCMoneyTransfer.Data.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ABCMoneyTransfer.Data.Repositories;

public interface IAuthRepository
{
    Task Register(RegisterVM model);
    Task<string> GetRoleByUser(ApplicationUser user);
    Task<ApplicationUser> GetUserByUsername(string username);
    Task<bool> Login(LoginVM loginVM);
    Task Logout();
    Task CreateRole(string role);
    Task<bool> CheckIfUsernameExists(string username);
    Task<IdentityResult> RegisterWithoutPassword(ApplicationUser user, string role);
    Task ChangePassword(ResetPasswordModel resetPasswordModel);
    Task ResetPassword(ResetPasswordModel resetPasswordModel);
    Task UnRegisterUser(string id);

    Task<IEnumerable<Users>> GetUsers();

    Task<List<ApplicationUserRole>> GetRoles();
}

public class AuthRepository(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationUserRole> roleManager,
    SignInManager<ApplicationUser> signInManager,
    IHttpContextAccessor httpContextAccessor
    )
    : IAuthRepository
{
   

    public async Task Register(RegisterVM model)
    {
        ValidateRegisterModel(model);

        var registeredUser = new ApplicationUser
        {
            UserName = model.Username,
            Name = model.Name
        };

        var role = await roleManager.FindByIdAsync(model.RoleId)
                   ?? throw new InvalidOperationException($"Role with ID '{model.RoleId}' not found.");

        var result = await userManager.CreateAsync(registeredUser, model.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Failed to register user.");
        }

        await userManager.AddToRoleAsync(registeredUser, role.Name!);
    }

    public async Task<string> GetRoleByUser(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        return roles.FirstOrDefault() ?? throw new InvalidOperationException("User has no assigned role.");
    }

    public async Task<ApplicationUser> GetUserByUsername(string username)
    {
        return await userManager.FindByNameAsync(username)
               ?? throw new InvalidOperationException($"User with username '{username}' not found.");
    }

    public async Task<bool> Login(LoginVM loginVM)
    {
        var result = await signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, isPersistent: true, lockoutOnFailure: false);
        return result.Succeeded;
    }

    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }

    public async Task CreateRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role name cannot be null or empty.", nameof(role));

        var roleExists = await roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            var result = await roleManager.CreateAsync(new ApplicationUserRole { Name = role });
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to create role.");
            }
        }
    }

    public async Task<bool> CheckIfUsernameExists(string username)
    {
        var user = await userManager.FindByNameAsync(username);
        return user != null;
    }

    public async Task<IdentityResult> RegisterWithoutPassword(ApplicationUser user, string role)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role cannot be null or empty.", nameof(role));

        var result = await userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
        return result;
    }

    public async Task ChangePassword(ResetPasswordModel resetPasswordModel)
    {
        ValidateResetPasswordModel(resetPasswordModel);

        var loggedInUser = GeneralUtility.GetUsernameFromClaim(httpContextAccessor);
        var user = await userManager.FindByNameAsync(loggedInUser)
                   ?? throw new InvalidOperationException("User not found.");

        var result = await userManager.ChangePasswordAsync(user, resetPasswordModel.OldPassword, resetPasswordModel.Password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);
        }
    }

    public async Task ResetPassword(ResetPasswordModel resetPasswordModel)
    {
        ValidateResetPasswordModel(resetPasswordModel);

        var user = await userManager.FindByNameAsync(GeneralUtility.GetUsernameFromClaim(httpContextAccessor))
                   ?? throw new InvalidOperationException("User not found.");

        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, resetToken, resetPasswordModel.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description);
        }
    }

    public async Task UnRegisterUser(string id)
    {
        ApplicationUser user = await userManager.FindByIdAsync(id) ?? throw new InvalidOperationException($"User with ID '{id}' not found.");

        string userName = GeneralUtility.GetUsernameFromClaim(httpContextAccessor);

        if (user.UserName == userName)
            throw new InvalidOperationException("You cannot delete yourself.");

        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            await userManager.RemoveFromRoleAsync(user, role);
        }

        await userManager.DeleteAsync(user);
    }

    public async Task<IEnumerable<Users>> GetUsers()
    {
        var users = await userManager.Users.ToListAsync();
        List<Users> usersList = [];

        foreach (var applicationUser in users)
        {
            usersList.Add(new Users()
            {
                Name = applicationUser.Name,
                Username = applicationUser.UserName!,
                Id = applicationUser.Id,
                Role = string.Join(", ", await userManager.GetRolesAsync(applicationUser)) 
            });
        }

        return usersList;
    }

    public async Task<List<ApplicationUserRole>> GetRoles()
    {
        var roles =  roleManager.Roles;
        return await roles.ToListAsync();
    }

    private static void ValidateRegisterModel(RegisterVM model)
    {
        if (string.IsNullOrWhiteSpace(model.RoleId)) throw new ArgumentException("Role is required.", nameof(model.RoleId));
        if (string.IsNullOrWhiteSpace(model.Username)) throw new ArgumentException("Username is required.", nameof(model.Username));
        if (string.IsNullOrWhiteSpace(model.Password)) throw new ArgumentException("Password is required.", nameof(model.Password));
        if (!string.Equals(model.Password, model.ConfirmPassword)) throw new ArgumentException("Password and Confirm Password do not match.", nameof(model.ConfirmPassword));
    }

    private static void ValidateResetPasswordModel(ResetPasswordModel model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model), "Reset password model cannot be null.");
        if (string.IsNullOrWhiteSpace(model.Password)) throw new ArgumentException("Password is required.", nameof(model.Password));
        if (!string.Equals(model.Password, model.ConfirmPassword)) throw new ArgumentException("Password and Confirm Password do not match.", nameof(model.ConfirmPassword));
    }
}
