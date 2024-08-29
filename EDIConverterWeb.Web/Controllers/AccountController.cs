using EDIConverterWeb.Data;
using EDIConverterWeb.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EDIConverterWeb.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("adduser")]
        public IsValidAddUserDataViewModel AddUser(AddUserViewModel vm)
        {
            var repo = new UsersRepo(_connectionString);
            var userExists = repo.UserExistsForThisEmail(vm.Email);
            var passwordsMatch = vm.Password == vm.PasswordConfirmation;
            if (!userExists && passwordsMatch)
            {
                repo.AddUser(new User
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = vm.Email
                }, vm.Password);
            }
            return new()
            {
                UserExistsForThisEmail = userExists,
                PasswordsMatch = passwordsMatch
            };
        }

        [HttpPost]
        [Route("login")]
        public User Login(LoginViewModel vm)
        {
            var repo = new UsersRepo(_connectionString);
            var user = repo.Login(vm.Email, vm.Password);
            if (user == null)
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim("user", vm.Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return user;
        }

        [HttpGet]
        [Route("getcurrentuser")]
        public User GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            var repo = new UsersRepo(_connectionString);
            return repo.GetByEmail(User.Identity.Name);
        }

        [HttpPost]
        [Route("logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync().Wait();
        }
    }
}
