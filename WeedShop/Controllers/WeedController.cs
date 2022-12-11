using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WeedShop.Controllers
{
    public class WeedController : Controller
    {
        private readonly IWeedService _weedService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;
        private static UserEntity? _user;
        private static WeedEntity? _weed;
        private static List<WeedEntity>? _weeds;
       
        public WeedController(IWeedService weedService, IFileService fileService, IUserService userService, IReviewService reviewService)
        {
            _weedService = weedService;
            _fileService = fileService;
            _userService = userService;
            if(_weeds is null)  _weeds = _weedService.GetAllWeeds();
            _reviewService = reviewService;
        }

        // GET: WeedController
        [AllowAnonymous]
        public async Task<IActionResult> Index(int AmountItemToAdd, int weedId)
        {
            //ViewBag.NameSortParm = categories;
            UserEntity user = null;
            if (User.Identity.IsAuthenticated)
            {
                 user = await _userService.GetUserByEmailAsync(HttpContext.User.FindFirst(ClaimTypes.Email).Value);
                if (AmountItemToAdd > 0 && AmountItemToAdd != null)
                {
                    var weed = await _weedService.GetWeedByIdAsync(weedId);
                    for (int i = 0; i < AmountItemToAdd; i++)
                    {
                        await _userService.AddWeedsToUserAsync(user.Id, weed);
                    }
                }
                // to set the icon of amount items on shoppingcart
                ViewData["WeedsFromUser"] = HomeController.weedsFromUser;
            }
           
            ViewData["Weeds"] = _weedService.GetAllWeeds();
            return View(_weeds);
        }
        [HttpPost]
        public IActionResult PostIndex(string categories)
        {
            //_weeds = _weedService.GetAllWeeds();
            switch (categories)
            {
                case "WEEDOOIL":
                    _weeds = _weedService.GetAllWeeds().Where(x => x.TypeProduct == TypeProduct.WEEDOIL).ToList();
                    break;
                case "WEED":
                    _weeds = _weedService.GetAllWeeds().Where(x => x.TypeProduct == TypeProduct.WEED).ToList();
                    break;
                case "HASH":
                    _weeds = _weedService.GetAllWeeds().Where(x => x.TypeProduct == TypeProduct.HASH).ToList();
                    break;
                default:
                    _weeds = _weedService.GetAllWeeds();
                    break;
            }
            return RedirectToAction("Index",_weeds);
        }
        public IActionResult Search(string searchWeedByName)
        {
            string searchQuery = searchWeedByName;
            // Perform search and return results
            if(string.IsNullOrEmpty(searchQuery))
            {
                _weeds = _weedService.GetAllWeeds();
                return  RedirectToAction("Index", _weedService.GetAllWeeds());
            }
            _weeds = _weeds.Where(x => x.Name.Contains(searchWeedByName)).ToList();
          return RedirectToAction("Index",_weeds);
        }
        // GET: WeedController/Details/5
        // [Authorize(Roles = "Admin")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> Details(int id)
        {
            var restaurant = await _weedService.GetWeedByIdAsync(id);
            if(restaurant is null)
            {
                return NotFound();
            }
            ViewData["Reviews"] = await _reviewService.GetReviewsFromWeedIDAsync(id);
            return View( await _weedService.GetWeedByIdAsync(id));
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
                _weeds = _weedService.GetAllWeeds();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WeedController/Edit/5
        [Authorize(Roles = "Admin")]
        public  async Task<IActionResult> Edit(int id)
        {
            return View( await _weedService.GetWeedByIdAsync(id));
        }

        // POST: WeedController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, WeedEntity weed)
        {
            try
            {
                //var weedy =  _weedService.GetWeedById(id);
                //weedy.Name = weed.Name;
                //_weeds = _weedService.GetAllWeeds();
                weed.Id = id;
                 _weedService.UpdateWeed(weed);
                return RedirectToAction(nameof(Details), new { id = weed.Id});
            }
            catch
            {
                return View();
            }
        }

        // GET: WeedController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await _weedService.GetWeedByIdAsync(id));
        }

        // POST: WeedController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, WeedEntity weed)
        {
            try
            {
                _weedService.DeleteWeedById(id);
                _weeds = _weedService.GetAllWeeds();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
  
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddtoCard(int id)
        {
            var user = await _userService.GetUserByEmailAsync(HttpContext.User.FindFirst(ClaimTypes.Email).Value);
            var weed =  await _weedService.GetWeedByIdAsync(id);
            await _userService.AddWeedsToUserAsync(user.Id, weed);
            _weeds = _weedService.GetAllWeeds();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Review(int id)
        {
            _user = await _userService.GetUserByEmailAsync(HttpContext.User.FindFirst(ClaimTypes.Email).Value);
            _weed = await _weedService.GetWeedByIdAsync(id);
            ViewData["User"] = _user;
            ViewData["Weed"] = _weed;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewPost(ReviewEntity review)
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            
            await _userService.AddReviewToWeedAsync(review);
            return RedirectToAction(nameof(Details), new { id = review.WeedId });
        }
    }
}
