using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WeedShop.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace WeedShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        private readonly IWeedService _weedService;
        public static UserEntity? User = new UserEntity();
        public HomeController(ILogger<HomeController> logger, IUserService userService, IWeedService weedService)
        {
            _logger = logger;
            _userService = userService;
            _weedService = weedService;
         
        }

        public  IActionResult Index()
        {
            var weedsPopular = ( _weedService.GetAllWeeds()).ToList();
            ViewData["MostPopularWeed"] = weedsPopular.Skip(1).Take(5).ToList();
            ViewData["MostPopularHash"] = weedsPopular.Skip(6).Take(5).ToList();
            User = _userService.GetUserByEmail(HttpContext.User.FindFirst(ClaimTypes.Email).Value);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Validate(UserEntity user)
        {
            var userLogin  = _userService.GetUser(user.FirstName, user.Password);
            if (userLogin is null) return NotFound();
            // todo place hashed in seperate method
            var sha1 = new SHA1CryptoServiceProvider();
            var userPasswordData = Encoding.ASCII.GetBytes(user.Password);
            var userFromDbData = Encoding.ASCII.GetBytes(userLogin.Password);
            var userFromDb = sha1.ComputeHash(userFromDbData);
            var userHashOne = sha1.ComputeHash(userPasswordData);
            var one = Convert.ToBase64String(userFromDb);
            var two = Convert.ToBase64String(userHashOne);
            Console.WriteLine(one);
            Console.WriteLine(two);
            if(user.FirstName == userLogin.FirstName && one == two)
            {
                var claims = new List<Claim>();
     
                claims.Add(new Claim("FirstName", userLogin.FirstName));
                claims.Add(new Claim(ClaimTypes.Name, userLogin.FirstName));
                claims.Add(new Claim("Email", userLogin.Email));
                claims.Add(new Claim(ClaimTypes.Email, userLogin.Email));
                if (userLogin.Role == "Admin")
                {
                    claims.Add(new Claim("Admin", userLogin.Role));
                    claims.Add(new Claim(ClaimTypes.Role, userLogin.Role));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Index");
            }
            return BadRequest();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public  IActionResult ValidateRegister(UserEntity user)
        {
            _userService.CreateUser(user);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ShoppingCart()
        {
            return View(User);
        }
       public IActionResult RemoveWeed(int id)
        {
       
            while (User.Weeds.Contains(_weedService.GetWeedById(id)))
            {
                _userService.DeleteWeeds(User.Id, id);
            }
            return RedirectToAction("ShoppingCart");
        }
    }
}