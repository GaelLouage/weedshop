using Infrastructuur.Database.Database;
using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Classes
{
    [Authorize(Roles = "Admin")]
    public class WeedService : IWeedService
    {
     
        private readonly WeedDbContext _weedDbContext;
        public WeedService( WeedDbContext weedDbContext)
        {
            _weedDbContext = weedDbContext;
        }

        public WeedEntity CreateWeed(WeedEntity weed)
        {
            _weedDbContext.Weeds.Add(weed);
            _weedDbContext.SaveChanges();
            return weed;
        }

        public void DeleteWeedById(int id)
        {
            _weedDbContext.Weeds.Remove(_weedDbContext.Weeds.FirstOrDefault(x => x.Id == id));
            _weedDbContext.SaveChanges();
        }
        public List<WeedEntity> GetAllWeeds()
        {
            return _weedDbContext.Weeds.ToList();
        }

        public async Task<WeedEntity> GetWeedByIdAsync(int id)
        {
            return await _weedDbContext.Weeds.FirstOrDefaultAsync(x => x.Id == id);
        }

        public WeedEntity UpdateWeed(WeedEntity weed)
        {
            var weedM = _weedDbContext.Weeds.FirstOrDefault(x => x.Id == weed.Id);
            
            weedM.THC = weed.THC;
            weedM.Name = weed.Name;
            weedM.Price = weed.Price;
            weedM.ImageFileLocation = weed.ImageFileLocation;
            weedM.Info = weed.Info;
            weedM.TypeProduct = weed.TypeProduct;

            _weedDbContext.Weeds.Update(weedM);
            _weedDbContext.SaveChanges();
            return weed;
        }
    }
 
}
