using Infrastructuur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Interfaces
{
    public interface IAddressService
    {
        Task<List<AddressEntity>> GetAddresses();
        Task<AddressEntity> GetAddressById(int id);
        
        Task<bool> AddAddressToUserByUserId(int id);
        Task<bool> RemoveAddressFromUserByUserId(int id, int addressId);
        Task<List<AddressEntity>> GetAllAddressesFromUserById(int userId);
        Task<AddressEntity> UpdateAddress(int addressId ,AddressEntity address);
    }
}
