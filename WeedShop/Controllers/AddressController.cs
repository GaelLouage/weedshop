using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WeedShop.Controllers
{
    public class AddressController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;

        private static UserEntity? user;
        public AddressController(IUserService userService, IAddressService addressService)
        {
            _userService = userService;
            _addressService = addressService;

        }

        public async Task<IActionResult> Index(int id)
        {
            user = await _userService.GetUserByEmailAsync(HttpContext.User?.FindFirst(ClaimTypes.Email)?.Value);
            var userAddress = await _addressService.GetAllAddressesFromUserById(user.Id);

            if (userAddress is null) return View();
            return View(userAddress);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var address = await _addressService.GetAddressById(id);
            if (address is null) return RedirectToAction(nameof(Index));
            return View(address);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id, UserEntity u)
        {
            if (user is null) return View();
            if (!await _addressService.RemoveAddressFromUserByUserId(user.Id, id))
            {
                return RedirectToAction(nameof(Delete));
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var address = await _addressService.GetAddressById(id);
            if (address is null)
            {
                return View();
            }
            return View(address);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([FromForm]AddressEntity addresst)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Invalid";
                return RedirectToAction(nameof(Edit));
            }
            var addressToUpdate = await _addressService.UpdateAddress(user.Id, addresst);
            if(addressToUpdate is null) return View();
            return RedirectToAction(nameof(Index));
        }
    }
}
