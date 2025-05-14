using System.Linq.Expressions;
using Cinema.Models;

namespace Cinema.Repositories.IRepositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
       Movie? GetOne(
       Expression<Func<Movie, bool>> expression,
       Func<IQueryable<Movie>, IQueryable<Movie>>? include = null,
       bool noTracking = true
            );


        IEnumerable<Movie> Get(
            Expression<Func<Movie, bool>>? expression = null,
            Func<IQueryable<Movie>, IQueryable<Movie>>? include = null,
            bool noTracking = true
            );
    }
}