using Infrastructuur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Interfaces
{
    public interface IWeedService
    {
        List<WeedEntity> GetAllWeeds();

        Task<WeedEntity> GetWeedByIdAsync(int id);
        WeedEntity CreateWeed(WeedEntity weed);
        void DeleteWeedById(int id);    
        WeedEntity UpdateWeed(WeedEntity weed);
    }
}
