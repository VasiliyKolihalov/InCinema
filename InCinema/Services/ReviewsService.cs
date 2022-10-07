using AutoMapper;
using InCinema.Constants;
using InCinema.Exceptions;
using InCinema.Models.Reviews;
using InCinema.Models.Roles;
using InCinema.Models.Users;
using InCinema.Repositories;

namespace InCinema.Services;

public class ReviewsService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    public ReviewsService(IApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    public IEnumerable<ReviewView> GetAll()
    {
        IEnumerable<Review> reviews = _applicationContext.Reviews.GetAll();
        return _mapper.Map<IEnumerable<ReviewView>>(reviews);
    }

    public IEnumerable<ReviewView> GetByUserId(int userId)
    {
        _applicationContext.Users.GetById(userId);
        IEnumerable<Review> reviews = _applicationContext.Reviews.GetByUserId(userId);
        return _mapper.Map<IEnumerable<ReviewView>>(reviews);
    }

    public ReviewView GetById(int reviewId)
    {
        Review review = _applicationContext.Reviews.GetById(reviewId);
        return _mapper.Map<ReviewView>(review);
    }

    public ReviewView Create(ReviewCreate reviewCreate, int userId)
    {
        User user = _applicationContext.Users.GetById(userId);
        if (user.IsConfirmEmail == false)
            throw new BadRequestException("Only users with verified email can create reviews");

        _applicationContext.Movies.GetById(reviewCreate.MovieId);

        _ = _applicationContext.Reviews.GetUserReview(reviewCreate.MovieId, userId)
            ?? throw new BadRequestException("Review already exist");

        var newReview = _mapper.Map<Review>(reviewCreate);
        newReview.DateTime = DateTime.Now;
        newReview.Author = _applicationContext.Users.GetById(userId);

        _applicationContext.Reviews.Add(newReview);

        return _mapper.Map<ReviewView>(newReview);
    }

    public ReviewView Update(ReviewUpdate reviewUpdate, int userId)
    {
        Review review = _applicationContext.Reviews.GetById(reviewUpdate.Id);

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(userId);
        if (roles.All(x => x.Name != RoleNames.UsersAdministrator) && review.Author.Id != userId)
            throw new ForbiddenException("User does not have enough rights for this action");

        var updateReview = _mapper.Map<Review>(reviewUpdate);
        _applicationContext.Reviews.Update(updateReview);

        var reviewView = _mapper.Map<ReviewView>(updateReview);
        reviewView.MovieId = review.MovieId;
        reviewView.Author = _mapper.Map<UserPreview>(review.Author);
        return reviewView;
    }

    public ReviewView Delete(int reviewId, int userId)
    {
        Review review = _applicationContext.Reviews.GetById(reviewId);

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(userId);
        if (roles.All(x => x.Name != RoleNames.UsersAdministrator) && review.Author.Id != userId)
            throw new ForbiddenException("User does not have enough rights for this action");

        _applicationContext.Reviews.Delete(review.Id);

        return _mapper.Map<ReviewView>(review);
    }
}