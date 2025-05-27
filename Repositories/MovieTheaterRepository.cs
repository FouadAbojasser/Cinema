using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class MovieTheaterRepository : Repository<MovieTheater>, IMovieTheaterRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public MovieTheaterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
