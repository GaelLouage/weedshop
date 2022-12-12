using Infrastructuur.Apis;
using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace WeedShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private static UserEntity? user;
        public static List<SelectListItem> CountriesListItem = new List<SelectListItem>();
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: UserController
        public async Task<ActionResult> Index()
        {

            return View(await _userService.GetAllUsersAsync());
        }
        // GET: UserController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if(user is null)
            {
                var userByEmail = await _userService.GetUserByEmailAsync(HttpContext.User?.FindFirst(ClaimTypes.Email)?.Value);
                return View(await _userService.GetUserByIdAsync(userByEmail.Id));
            }
            return View(user);
        }
        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserEntity user)
        {
            try
            {
                await _userService.CreateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await _userService.GetUserByIdAsync(id));
        }
        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserEntity user)
        {
            try
            {
                var userToEdit =  await _userService.UpdateUserAsync(id, user);
            
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await _userService.GetUserByIdAsync(id));
        }
        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, UserEntity user)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // addAddressToUser

        public async Task<ActionResult> AddAddressToUser(int userId)
        {
            user = await _userService.GetUserByIdAsync(userId);
            var addresses = CountryApi.Instance;
            if (addresses is not null)
            {
                ViewData["addresses"] = addresses;
            }
            foreach (var item in await addresses.GetCountriesAsync())
            {
                CountriesListItem.Add(new SelectListItem
                {
                    Value = item.Name,
                    Text = item.Name
                });
            }
            return View();
        }
        [HttpPost]
     //   [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAddressToUser(AddressEntity addressVM)
        {
            
            await _userService.AddAddressToUserAsync(user.Id, addressVM);
            return RedirectToAction("Detail");
        }
    }
}
