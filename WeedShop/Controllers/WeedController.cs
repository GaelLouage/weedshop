using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WeedShop.Controllers
{
    public class WeedController : Controller
    {
        private readonly IWeedService _weedService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
      
        public WeedController(IWeedService weedService, IFileService fileService, IUserService userService)
        {
            _weedService = weedService;
            _fileService = fileService;
            _userService = userService;
        }

        // GET: WeedController
        [AllowAnonymous]
        public IActionResult Index(int AmountItemToAdd, int weedId)
        {

            if(AmountItemToAdd > 0 && AmountItemToAdd != null)
            {
                var weed =  _weedService.GetWeedById(weedId);
                for(int i = 0; i < AmountItemToAdd; i++)
                {
                    _userService.AddWeedsToUser(_userService.GetUserByEmail(HttpContext.User.FindFirst(ClaimTypes.Email).Value).Id, weed);
                }
            }
            return View( _weedService.GetAllWeeds());
        }

        // GET: WeedController/Details/5
        // [Authorize(Roles = "Admin")]
        [Authorize(Roles ="Admin,User")]
        public IActionResult Details(int id)
        {
            var restaurant =  _weedService.GetWeedById(id);
            if(restaurant is null)
            {
                return NotFound();
            }
            return View( _weedService.GetWeedById(id));
        }

        // GET: WeedController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: WeedController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(WeedEntity weed, IFormFile image)
        {
            try
            {
                weed.ImageFileLocation = await _fileService.UploadImage(image);
                _weedService.CreateWeed(weed);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WeedController/Edit/5
        [Authorize(Roles = "Admin")]
        public  IActionResult Edit(int id)
        {
            return View( _weedService.GetWeedById(id));
        }

        // POST: WeedController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, WeedEntity weed)
        {
            try
            {
                var weedy =  _weedService.GetWeedById(id);
                weedy.Name = weed.Name;
                return RedirectToAction(nameof(Details), new { id = weed.Id});
            }
            catch
            {
                return View();
            }
        }

        // GET: WeedController/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            return View(_weedService.GetWeedById(id));
        }

        // POST: WeedController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, WeedEntity weed)
        {
            try
            {
                _weedService.DeleteWeedById(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
  
        [Authorize(Roles = "Admin,User")]
        public IActionResult AddtoCard(int id)
        {
            var weed =  _weedService.GetWeedById(id);
          
                _userService.AddWeedsToUser(_userService.GetUserByEmail(HttpContext.User.FindFirst(ClaimTypes.Email).Value).Id, weed);
          
            return RedirectToAction("Index");
        }
     
    }
}
