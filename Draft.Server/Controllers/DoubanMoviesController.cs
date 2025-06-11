using Draft.Models;
using Draft.Models.Dto.Movie;
using Draft.Server.Extensions;
using Draft.Server.Services;
using Draft.Server.Services.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Draft.Server.Controllers;

[ApiController]
[Route("api/v1/douban-movies")]
public class DoubanMoviesController(
    IMovieService movieService,
    IDateTimeProvider dateTimeProvider
) : ControllerBase {

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> PutMovieEntry(DoubanMovieModifyRequest request) {
        DoubanMovie movie = request.ToModel();
        if (string.IsNullOrWhiteSpace(movie.Year))
            movie.Year = dateTimeProvider.UtcNow.Year.ToString();

        MovieOperationResult result = await movieService.CreateMovieAsync(movie);
        return result
            ? CreatedAtAction(
                nameof(GetMovieEntry),
                new {
                    result.Content!.Id
                },
                result.Content!.ToResponse()
            )
            : BadRequest(result.Errors);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<ActionResult> DeleteMovieEntry(int id) {
        MovieOperationResult result = await movieService.DeleteMovieAsync(id);
        return result ? NoContent() : NotFound(result.Errors);
    }

    [HttpPost("{id:int}")]
    [Authorize]
    public async Task<ActionResult<DoubanMovieResponse>> PostMovieEntry(int id, DoubanMovieModifyRequest request) {
        MovieQueryResults queryResults = await movieService.FindMovieByIdAsync(id);
        if (!queryResults) return NotFound(queryResults.Errors);

        DoubanMovie movie = queryResults.Content!.First();
        request.Modify(movie);

        MovieOperationResult operationResult = await movieService.UpdateMovieAsync(movie);
        return operationResult ? Ok(operationResult.Content?.ToResponse()) : NotFound(operationResult.Errors);
    }

    [HttpGet]
    public ActionResult<IEnumerable<DoubanMovieResponse>> GetMovieEntries() =>
        Ok(movieService.GetMovies().Content!.Select(DoubanMovieExtension.ToResponse));

    [HttpGet("simple")]
    public ActionResult<IEnumerable<DoubanMovieSimpleResponse>> GetMovieSimple() =>
        Ok(movieService.GetMovies().Content!.Select(DoubanMovieExtension.ToSimpleResponse));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoubanMovieResponse>> GetMovieEntry(int id) {
        MovieQueryResults results = await movieService.FindMovieByIdAsync(id);
        return results ? Ok(results.Content!.First().ToResponse()) : NotFound(results.Errors);
    }

    [HttpGet("{id:int}/img")]
    public async Task<ActionResult> GetMoviePreviewImage(int id) {
        MovieQueryResults results = await movieService.FindMovieByIdAsync(id);

        byte[] bytes = Convert.FromBase64String(results.Content!.First().PreviewImageBase64);

        return results
            ? File(bytes, "image/jpeg")
            : NotFound(results.Errors);
    }
}
