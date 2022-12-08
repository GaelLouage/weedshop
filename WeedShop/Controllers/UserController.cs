using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Mvc;

namespace WeedShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
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
            return View(await _userService.GetUserByIdAsync(id));
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
    }
}
