using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WeedShop.Models;
using System.Security.Cryptography;
using System.Text;

namespace WeedShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        private readonly IWeedService _weedService;
        public static UserEntity? userLogin = null;
        public HomeController(ILogger<HomeController> logger, IUserService userService, IWeedService weedService)
        {
            _logger = logger;
            _userService = userService;
            _weedService = weedService;
        }
        public async  Task<IActionResult> Index()
        {
            var weedsPopular = ( _weedService.GetAllWeeds()).ToList();
            ViewData["MostPopularWeed"] = weedsPopular.Skip(1).Take(5).ToList();
            ViewData["MostPopularHash"] = weedsPopular.Skip(6).Take(5).ToList();

            userLogin = await _userService.GetUserByEmailAsync(HttpContext.User?.FindFirst(ClaimTypes.Email)?.Value);
            ViewData["WeedsFromUser"] = await _userService.GetWeedFromUserByUserId(userLogin.Id);

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
            // todo place hashed in seperate method
            var sha1 = new SHA1CryptoServiceProvider();
            var userPasswordData = Encoding.ASCII.GetBytes(user.Password);
            var userHashOne = sha1.ComputeHash(userPasswordData);

            user.Password =  Convert.ToBase64String(userHashOne);
            userLogin  = await _userService.GetUserAsync(user.FirstName,user.Password);

            if (userLogin is null) return NotFound();
          
            if (user.FirstName == userLogin.FirstName && user.Password ==  userLogin.Password)
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
                   
                } else
                {
                    claims.Add(new Claim("User", userLogin.Role));
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
            userLogin = null;
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
            try
            {
                var sha1 = new SHA1CryptoServiceProvider();
                var userPasswordData = Encoding.ASCII.GetBytes(user.Password);
         
                var userHashOne = sha1.ComputeHash(userPasswordData);
        
                var two = Convert.ToBase64String(userHashOne);
                user.Password = two;
                _userService.CreateUserAsync(user);
            } catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
         
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> ShoppingCart()
        {
            var user = await _userService.GetUserByEmailAsync(HttpContext.User.FindFirst(ClaimTypes.Email).Value);
            if (user is null)
            {

            }
            ViewData["WeedsFromUser"] = await _userService.GetWeedFromUserByUserId(user.Id);
            return View(user);
        }
        public async Task<IActionResult> RemoveWeed(int id)
        {
            var user = await _userService.GetUserByEmailAsync(HttpContext.User.FindFirst(ClaimTypes.Email).Value);
            if(user is null)
            {
                return BadRequest();
            }
            
              _userService.DeleteWeeds(user.Id, id);
          
            return RedirectToAction("ShoppingCart");
        }
        [Authorize(Roles ="Admin,User")]
        [HttpGet]
        public  async Task<IActionResult> Order(int id)
        {
            ViewData["Weeds"] = await _userService.GetWeedFromUserByUserId(id);
            return View(await _userService.GetUserByIdAsync(id));
        }
    }
}