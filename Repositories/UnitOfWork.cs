using Cinema.Repositories.IRepositories;

namespace Cinema.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbcontext;
        public IActorRepository Actor { get; private set; }
        public IGenreRepository Genre { get; private set; }
        public IDirectorRepository Director { get; private set; }
        public IMovieRepository Movie { get; private set; }
        public IReviewRepository MovieReviews { get; private set; }
        public IOTPRepository OTP { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IMovieRatesRepository MovieRates { get; private set; }
        public ITheaterRepository Theater { get; private set; }
        public IShowTimeRepository ShowTime {  get; private set; }
        public IMovieTheaterRepository MovieTheater { get; private set; }
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
            Actor = new ActorRepository(_dbcontext);
            Genre = new GenreRepository(_dbcontext);
            Director = new DirectorRepository(_dbcontext);
            Movie = new MovieRepository(_dbcontext);
            MovieReviews = new ReviewRepository(_dbcontext);
            OTP = new OTPRepository(_dbcontext);
            ApplicationUser = new ApplicationUserRepository(_dbcontext);
            MovieRates = new MovieRatesRepositorycs(_dbcontext);
            Theater = new TheaterRepository(_dbcontext);
            ShowTime = new ShowTimeRepository(_dbcontext);
            MovieTheater = new MovieTheaterRepository(_dbcontext);
        }
      
    }
}
