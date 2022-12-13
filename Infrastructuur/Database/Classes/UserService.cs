using Infrastructuur.Database.Database;
using Infrastructuur.Database.Interfaces;
using Infrastructuur.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Classes
{
    public class UserService : IUserService
    {
        private readonly WeedDbContext _weedDbContext;

        private readonly IWeedService _weedService;
        public UserService(WeedDbContext weedDatabase, IWeedService weedService)
        {
            _weedDbContext = weedDatabase;

            _weedService = weedService;
        }

        public async Task AddWeedsToUserAsync(int userId, WeedEntity weed)
        {
            //var userWeed = new UserWeedEntity();
            var user = await _weedDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var weedEn = await _weedDbContext.Weeds.FirstOrDefaultAsync(w => w.Id == weed.Id);
            if (user is null || weedEn is null)
            {

            }
            _weedDbContext.UserWeeds.Add(new UserWeedEntity
            {
                UserId = userId,
                WeedId = weed.Id
            });
            _weedDbContext.SaveChanges();
        }

        public async Task<bool> CreateUserAsync(UserEntity user)
        {
            user.Role = "User";
            var users = await _weedDbContext.Users.ToListAsync();
            if (!users.Any(x => x.Email == user.Email || (x.FirstName == user.FirstName && x.LastName == user.LastName)))
            {
                _weedDbContext.Users.Add(user);
                await _weedDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = _weedDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
            {

            }
            _weedDbContext.Users.Remove(user);
            await _weedDbContext.SaveChangesAsync();
        }

        public void DeleteWeeds(int userId, int weedId)
        {
            var weedsToRemoveFromUser = _weedDbContext.UserWeeds.Where(x => x.UserId == userId && x.WeedId == weedId).ToList();
            foreach (var weedToRemove in weedsToRemoveFromUser)
            {
                _weedDbContext.UserWeeds.Remove(weedToRemove);
            }
            _weedDbContext.SaveChanges();
        }

        public async Task<List<UserEntity>> GetAllUsersAsync()
        {
            var users = await _weedDbContext.Users.ToListAsync();
            if (users is null)
            {
                return null;
            }
            return users;
        }

        public async Task<UserEntity> GetUserAsync(string firstName, string password)
        {
            var user = await _weedDbContext.Users.FirstOrDefaultAsync(f => f.FirstName == firstName && f.Password == password);
            if (user is null)
            {

            }
            return user;
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            var user = await _weedDbContext.Users.FirstOrDefaultAsync(f => f.Email == email);
            if (user is null)
            {

            }
            return user;
        }

        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            var user = await _weedDbContext.Users.FirstOrDefaultAsync(f => f.Id == id);
            if (user is null)
            {

            }
            return user;
        }

        public async Task<List<WeedEntity>> GetWeedFromUserByUserId(int id)
        {
            var weedFromUser = await _weedDbContext.UserWeeds.Where(x => x.UserId == id).Select(x => x.Weed).ToListAsync();
            if (weedFromUser is null)
            {

            }
            return weedFromUser;
        }

        public async Task<UserEntity> UpdateUserAsync(int userId, UserEntity userVm)
        {
            var user = await _weedDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.FirstName = userVm.FirstName;
            user.LastName = userVm.LastName;
            user.UserAddress = userVm.UserAddress;
            if(userVm.Role is null) {
                user.Role = "User";
            } else
            {
                user.Role = userVm.Role;
            }
           
            if (user is null)
            {

            }
            _weedDbContext.Users.Update(user);
            await _weedDbContext.SaveChangesAsync();
            return user;
        }

        public async Task AddReviewToWeedAsync(ReviewEntity review)
        {
            _weedDbContext.Reviews.Add(review);
            await _weedDbContext.SaveChangesAsync();
        }

        public async Task<UserEntity> AddAddressToUserAsync(int userId, AddressEntity address)
        {
            var user = await GetUserByIdAsync(userId);
            if (user is null || address is null)
            {
                // return a dto with an error
                return null;
            }
            _weedDbContext.Addresses.Add(address);
            await _weedDbContext.SaveChangesAsync();

            var addressToAddToIntersectTable = _weedDbContext.UserAddnresses.Add(new UserAddressEntity
            {
                User = user,
                UserId = userId,
                Address = address,
                AddressId = address.Id
            });
            await _weedDbContext.SaveChangesAsync();
            return user;
        }
        public async Task<AddressEntity> GetFirstAddressFromUserAsync(int userId)
        {
            var userAddressId = (await _weedDbContext.UserAddnresses.FirstOrDefaultAsync(x => x.UserId == userId));
            if (userAddressId is null)
            {
                return null;
            }
            var address = await _weedDbContext.Addresses.FirstOrDefaultAsync(x => x.Id == userAddressId.AddressId);
            if (address is null) return null;
            return address;
        }

        public async Task<List<AddressEntity>> GetAddressesFromUserById(int userID)
        {
            var userAddresses = await _weedDbContext.UserAddnresses
                                .Where(x => x.UserId == userID)
                                .SelectMany(x => _weedDbContext.Addresses.Where(a => a.Id == x.AddressId))
                                .ToListAsync();
            return userAddresses;

        }
    }
}
