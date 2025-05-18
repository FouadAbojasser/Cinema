using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class OTPRepository : Repository<OTP>, IOTPRepository
    {
        private readonly ApplicationDbContext dbContext;
        public OTPRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;

        }
    }
}
