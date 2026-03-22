using HelloKitty.Application.Features.Reviews.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers
{
    [Route("api/reviews")]
    [Authorize]
    public class ReviewsController : BaseController
    {
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpDelete("{reviewId:int}")]
        public async Task<IActionResult> Delete(int reviewId, CancellationToken ct)
        {
            var result = await _reviewService.DeleteAsync(CurrentUserId, reviewId, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }
    }
}