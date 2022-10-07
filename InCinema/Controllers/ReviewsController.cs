using InCinema.Extensions;
using InCinema.Models.Reviews;
using InCinema.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ReviewsService _reviewsService;

    public ReviewsController(ReviewsService reviewsService)
    {
        _reviewsService = reviewsService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ReviewView>> GetAll()
    {
        return Ok(_reviewsService.GetAll());
    }

    [HttpGet("getByUser/{userId}")]
    public ActionResult<IEnumerable<ReviewView>> GetByUserId(int userId)
    {
        return Ok(_reviewsService.GetByUserId(userId));
    }

    [HttpGet("{reviewId}")]
    public ActionResult<ReviewView> Get(int reviewId)
    {
        return Ok(_reviewsService.GetById(reviewId));
    }

    [Authorize]
    [HttpPost]
    public ActionResult<ReviewView> Post(ReviewCreate reviewCreate)
    {
        ReviewView reviewView = _reviewsService.Create(reviewCreate, this.GetUserId());
        return Ok(reviewView);
    }

    [Authorize]
    [HttpPut]
    public ActionResult<ReviewView> Put(ReviewUpdate reviewUpdate)
    {
        ReviewView reviewView = _reviewsService.Update(reviewUpdate, this.GetUserId());
        return Ok(reviewView);
    }

    [Authorize]
    [HttpDelete("{reviewId}")]
    public ActionResult<ReviewView> Delete(int reviewId)
    {
        ReviewView reviewView = _reviewsService.Delete(reviewId, this.GetUserId());
        return Ok(reviewView);
    }
}