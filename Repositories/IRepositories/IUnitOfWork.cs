namespace Cinema.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IActorRepository Actor { get; }
        IGenreRepository Genre { get; }
        IDirectorRepository Director { get; }
        IMovieRepository Movie { get; }
        IReviewRepository MovieReviews { get; }
        IOTPRepository OTP { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IMovieRatesRepository MovieRates { get; }
    }
}
