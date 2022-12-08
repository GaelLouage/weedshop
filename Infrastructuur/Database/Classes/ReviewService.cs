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
    public class ReviewService : IReviewService
    {
        private readonly WeedDbContext _weedDbContext;

        public ReviewService(WeedDbContext weedDbContext)
        {
            _weedDbContext = weedDbContext;
        }

        public ReviewEntity AddReview(ReviewEntity rev)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReviewEntity>> GetReviewsFromWeedIDAsync(int weedID)
        {
            var reviews  = await _weedDbContext.Reviews.Where(x => x.WeedId == weedID).ToListAsync();
            if(reviews.Count == 0)
            {
                return new List<ReviewEntity>();
            }
            return reviews;
        }


  
        public async Task<List<ReviewEntity>> GetAllReviewsAsync()
        {
            return await _weedDbContext.Reviews.ToListAsync();
        }
    }
}
