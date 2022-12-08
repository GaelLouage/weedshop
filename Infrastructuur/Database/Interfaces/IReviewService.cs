using Infrastructuur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Interfaces
{
    public interface IReviewService
    {
        ReviewEntity AddReview(ReviewEntity rev);
    

        Task<List<ReviewEntity>> GetReviewsFromWeedIDAsync(int weedID);
        Task<List<ReviewEntity>> GetAllReviewsAsync();
    }
}
