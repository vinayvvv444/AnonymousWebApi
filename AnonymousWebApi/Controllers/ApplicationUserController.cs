using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AnonymousWebApi.Data.DomainModel;
using AnonymousWebApi.Helpers.EmailService;
using AnonymousWebApi.Models;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AnonymousWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _singInManager;
        private readonly ApplicationSettings _appSettings;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IEmailSender _emailSender;

        public ApplicationUserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<ApplicationSettings> appSettings,
            IBackgroundJobClient backgroundJobs,
            IRecurringJobManager recurringJobManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _singInManager = signInManager;
            _appSettings = appSettings.Value;
            _backgroundJobs = backgroundJobs;
            _recurringJobManager = recurringJobManager;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("Register")]
        //POST : /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {
            model.Role = "Admin";
            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.Password).ConfigureAwait(false);
                await _userManager.AddToRoleAsync(applicationUser, model.Role).ConfigureAwait(false);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Login service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
            {
                //Get role assigned to the user
                var role = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                // _backgroundJobs.Enqueue(() => Console.WriteLine("Welcome"));
                // _backgroundJobs.Schedule(() => Console.WriteLine("scheduled job Welcome"),TimeSpan.FromMinutes(2));
                //_recurringJobManager.AddOrUpdate("recurring job", () => Console.WriteLine("recurring job"), "* * * * *");

                // var message = new Message(new string[] { "vinayvvv444@gmail.com" }, "Test email", "This is the content from our email.");
                //_emailSender.SendEmail(message);
               // _emailSender.SendMail();

                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }
    }
}