using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema;

namespace Cinema.Repositories
{
    public class TheaterRepository : Repository<Theater>, ITheaterRepository

    {
        public TheaterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
