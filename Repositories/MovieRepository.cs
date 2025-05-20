using System.Linq.Expressions;
using System.Linq;
using Cinema;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Movie? GetOne(
            Expression<Func<Movie, bool>> expression,
            Func<IQueryable<Movie>, IQueryable<Movie>>? include = null,
            bool noTracking = true)
        {
            IQueryable<Movie> query = dbContext.Movies;

            if (noTracking)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return query.FirstOrDefault(expression);
        }

        public IEnumerable<Movie> Get(
            Expression<Func<Movie, bool>>? expression = null,
            Func<IQueryable<Movie>, IQueryable<Movie>>? include = null,
            bool noTracking = true)
        {
            IQueryable<Movie> query = dbContext.Movies;

            if (noTracking)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            if (expression != null)
                query = query.Where(expression);

            return query.ToList();
        }
    }

}
