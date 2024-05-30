using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Data.Identity;
using Tomasos_Pizzeria.Domain.DTO;

namespace Tomasos_Pizzeria.Core.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUserService> _logger;
        private readonly IConfiguration _configuration;

        public ApplicationUserService(SignInManager<ApplicationUser> signInManager,
                                        UserManager<ApplicationUser> userManager,
                                        ILogger<ApplicationUserService> logger,
                                        IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> AddUserAsync(UserDTO user)
        {
            _logger.LogError("La till kund");//För att se att jag kan logga till Application Insights
            if (user.Username is null || user.Password is null)
                return false;
            try
            {
                var existingUser = await _userManager.FindByNameAsync(user.Username);
                if (existingUser != null)
                    return false; // Användarnamnet är redan taget, returnera false

                // Skapa en ny användare
                var newUser = new ApplicationUser()
                {
                    UserName = user.Username,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNr
                };

                // Lägg till den nya användaren
                var result = await _userManager.CreateAsync(newUser, user.Password);

                // Om skapandet av användaren lyckades
                if (result.Succeeded)
                {
                    // Lägg till användaren i rollen
                    var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "RegularUser");

                    // Om tillägg till rollen lyckades
                    if (addToRoleResult.Succeeded)
                        return true; // Bekräfta transaktionen och returnera sant
                }

                // Ångra transaktionen om något misslyckades
                await _userManager.DeleteAsync(newUser);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing Create New User.");
                return false;
            }
        }

        public async Task<bool> UpgradeToPremiumAsync(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user is null) return false;

                var currentRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                // Lägg till användaren i den nya rollen
                var result = await _userManager.AddToRoleAsync(user, "PremiumUser");

                if (result.Succeeded)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing Upgrade_To_Premium_Async.");
                throw;
            }

        }

        public async Task<bool> UpdateUserAsync(UserDTO user, string id)
        {
            try
            {
                // Hitta användaren med ID
                var selectedUser = await _userManager.FindByIdAsync(id);

                if (selectedUser == null)
                    return false;

                // Kontrollera om det nya användarnamnet redan finns
                var checkUsername = await _userManager.FindByNameAsync(user.Username);

                // Kontrollera att användarnamnet inte är upptaget av någon annan än den nuvarande användaren
                if (checkUsername != null && checkUsername.Id != id)
                    return false;

                // Uppdatera användaregenskaper
                selectedUser.UserName = user.Username;
                selectedUser.Email = user.Email;
                selectedUser.PhoneNumber = user.PhoneNr;

                // Uppdatera användaren i databasen
                var result = await _userManager.UpdateAsync(selectedUser);

                if (!result.Succeeded)
                     return false;


                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing Update User.");
                throw;
            }
        }
        public async Task<bool> ChangePasswordAsync(string newPassword, string userId)
        {
            try
            {
                var selectedUser = await _userManager.FindByIdAsync(userId);
                if (selectedUser == null) return false;

                // Generate a password reset token
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(selectedUser);

                // Reset the password
                var result = await _userManager.ResetPasswordAsync(selectedUser, resetToken, newPassword);
                if (!result.Succeeded)
                {
                    // Logga felmeddelanden från resultatet
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Fel: {error.Description}");
                    }
                    return false;
                }

                return result.Succeeded;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing UpdatePassword.");
                throw;
            }
        }

        public async Task<string> UserLoginAsync(string username, string password)
        {
            _logger.LogError("Någon loggar in"); //För att se att jag kan logga till Application Insights
            try
            {
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(username);

                    if (user != null)
                        return await CreateToken(user);
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing user login.");
                throw;
            }
        }

        public async Task<bool> DownGradeToRegularAsync(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user is null) return false;

                var currentRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                // Lägg till användaren i den nya rollen
                var result = await _userManager.AddToRoleAsync(user, "RegularUser");

                if (result.Succeeded)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing Upgrade_To_Premium_Async.");
                throw;
            }
        }

        public async Task<List<ApplicationUserWithRolesDTO>> GetAllUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<ApplicationUserWithRolesDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new ApplicationUserWithRolesDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Points = user.Points,
                    Roles = roles.ToList()
                });
            }

            return userRoles;
        }
        private async Task<string> CreateToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Lägg till roller som anspråk
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey.Key));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            //Skapa options för att sätta upp en token
            var tokenOptions = new JwtSecurityToken(
                    issuer: SecretKey.IssuerAudience,
                    audience: SecretKey.IssuerAudience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(90),
                    signingCredentials: signinCredentials);

            //Generar en ny token som skall skickas tillbaka 
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}
