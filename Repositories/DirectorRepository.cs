using Cinema;
using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class DirectorRepository : Repository<Director>, IDirectorRepository
    {
        private readonly ApplicationDbContext dbContext;

        //Add special Implementation for Director
        public DirectorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
