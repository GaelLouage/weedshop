using Infrastructuur.Database.Database;
using Infrastructuur.Database.Interfaces;
using Infrastructuur.Mappers;
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
    [Authorize(Roles = "Admin,User")]
    public class AddressService : IAddressService
    {
        private readonly WeedDbContext _weedDbContext;
        

        public AddressService(WeedDbContext weedDbContext)
        {
            _weedDbContext = weedDbContext;
        }

        public Task<bool> AddAddressToUserByUserId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AddressEntity> GetAddressById(int id)
        {
            return await _weedDbContext.Addresses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AddressEntity>> GetAddresses()
        {
            return await _weedDbContext.Addresses.ToListAsync();
        }
        public async Task<List<AddressEntity>> GetAllAddressesFromUserById(int userId)
        {
            //var userAddres = await _weedDbContext.UserAddnresses.Where(x => x.UserId == userId).ToListAsync();
            var userAddresses = await _weedDbContext.UserAddnresses
                               .Where(x => x.UserId == userId)
                               .SelectMany(x => _weedDbContext.Addresses.Where(a => a.Id == x.AddressId))
                               .ToListAsync();


            return userAddresses;
        }
        public async Task<bool> RemoveAddressFromUserByUserId(int id, int addressId)
        {
            //to delete address from specific user
            var user = await _weedDbContext.Users.FirstOrDefaultAsync(x => x.Id  == id);
            if(user is null)
            {
                return false;
            }
            var userAddressIntersect = await _weedDbContext.UserAddnresses.Where(x => x.AddressId == addressId && x.UserId == id).ToListAsync();
            if(userAddressIntersect is null)
            {
                return false;
            }
            _weedDbContext.UserAddnresses.RemoveRange(userAddressIntersect);
          
            var address =  _weedDbContext.Addresses.FirstOrDefault(x => x.Id == addressId);

            _weedDbContext.Addresses.Remove(address);
            if(await _weedDbContext.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<AddressEntity> UpdateAddress(int addressId, AddressEntity address)
        {
            var addressToUpdate = await _weedDbContext.Addresses.FirstOrDefaultAsync(x => x.Id == address.Id);
            if (addressToUpdate is null) return null;

            addressToUpdate.Address = address.Address;
            addressToUpdate.AddressNumber = address.AddressNumber;
            addressToUpdate.City = address.City;
            addressToUpdate.Country = address.Country;
            addressToUpdate.PostalCode = address.PostalCode;


            _weedDbContext.Addresses.Update(addressToUpdate);
            await _weedDbContext.SaveChangesAsync();
            return addressToUpdate;
        }
    }
}
