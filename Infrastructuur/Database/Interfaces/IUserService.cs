using Infrastructuur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Interfaces
{
    public  interface IUserService
    { 
        Task<List<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserAsync(string firstName, string password);

        Task CreateUserAsync(UserEntity user);
 

        Task<UserEntity> GetUserByIdAsync(int id);

        Task DeleteUserAsync(int id);    

        Task AddWeedsToUserAsync(int userId,WeedEntity weed);
        Task<UserEntity> GetUserByEmailAsync(string email);
        void DeleteWeeds(int userId, int weedId);
        Task<UserEntity> UpdateUserAsync(int userId, UserEntity user);
        Task<List<WeedEntity>> GetWeedFromUserByUserId(int id);
       
    }
}
