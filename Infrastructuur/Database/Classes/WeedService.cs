using Infrastructuur.Database.Database;
using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly WeedDatabase _weedDatabase;

        public WeedService(WeedDatabase weedDatabase)
        {
            _weedDatabase = weedDatabase;
        }

        public WeedEntity CreateWeed(WeedEntity weed)
        {
            weed.Id = _weedDatabase.Weeds.Max(x => x.Id) + 1;
            _weedDatabase.Weeds.Add(weed);
            return weed;
        }

        public void DeleteWeedById(int id)
        {
            _weedDatabase.Weeds.Remove(_weedDatabase.Weeds.FirstOrDefault(x => x.Id == id));
        }

   

        public List<WeedEntity> GetAllWeeds()
        {
            return _weedDatabase.Weeds;
        }

        public WeedEntity GetWeedById(int id)
        {
            return _weedDatabase.Weeds.FirstOrDefault(x => x.Id == id);
        }


    }
}
