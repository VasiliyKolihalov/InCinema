using InCinema.Models.Reviews;

namespace InCinema.Repositories.Reviews;

public interface IReviewsRepository : IRepository<Review, int>
{
    public IEnumerable<Review> GetByMovieId(int movieId);
    public IEnumerable<Review> GetByUserId(int userId);
    public Review? GetUserReview(int movieId, int userId);
}