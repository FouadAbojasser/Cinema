using Cinema;
using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class ReviewRepository : Repository<MovieReviews>, IReviewRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
