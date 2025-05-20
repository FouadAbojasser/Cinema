using System.Linq.Expressions;
using Cinema;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repositories
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ActorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
