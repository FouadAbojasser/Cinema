using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class MovieRatesRepositorycs : Repository<MovieRates>, IMovieRatesRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MovieRatesRepositorycs(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
