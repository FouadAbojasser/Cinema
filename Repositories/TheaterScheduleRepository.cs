using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class TheaterScheduleRepository : Repository<TheaterSchedule>, ITheaterScheduleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TheaterScheduleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
