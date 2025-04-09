using Firebase_Auth.Helper.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Firebase_Auth.Controllers;


[ApiController]
public abstract class CoreController : ControllerBase
{
  protected readonly ILogger<CoreController> _logger;

  public CoreController(ILogger<CoreController> logger)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }
  #region Success Responses
  protected IActionResult ToSuccess(string message, object? data = null)
  {
    return Ok(new ApiResponse<object>
    {
      Success = true,
      StatusCode = StatusCodes.Status200OK,
      Message = message,
      Data = data
    });
  }

  protected IActionResult ToCreated(Guid id, string message, object? data = null, string? routeName = null, object? routeValues = null)
  {
    var response = new ApiResponse<object>
    {
      Success = true,
      StatusCode = StatusCodes.Status201Created,
      Message = message,
      Data = data
    };

    if (routeName != null)
      return CreatedAtRoute(routeName, routeValues, response);

    return StatusCode(StatusCodes.Status201Created, response);
  }

  protected IActionResult ToNoContent()
  {
    return NoContent();
  }
  #endregion

  #region Error Responses
  protected IActionResult ToNotFound(string message)
  {
    return NotFound(new ApiResponse<object>
    {
      Success = false,
      StatusCode = StatusCodes.Status404NotFound,
      Message = message
    });
  }

  protected IActionResult ToBadRequest(string message)
  {
    return BadRequest(new ApiResponse<object>
    {
      Success = false,
      StatusCode = StatusCodes.Status400BadRequest,
      Message = message
    });
  }

  protected IActionResult ToBadRequest(ModelStateDictionary modelState)
  {
    var errors = modelState.Values
        .SelectMany(v => v.Errors)
        .Select(e => e.ErrorMessage)
        .ToList();

    return BadRequest(new ApiResponse<object>
    {
      Success = false,
      StatusCode = StatusCodes.Status400BadRequest,
      Message = "Validation failed",
      Errors = errors
    });
  }

  protected IActionResult ToUnauthorized(string message = "Unauthorized access")
  {
    return Unauthorized(new ApiResponse<object>
    {
      Success = false,
      StatusCode = StatusCodes.Status401Unauthorized,
      Message = message
    });
  }

  protected IActionResult ToForbidden(string message = "Access forbidden")
  {
    return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<object>
    {
      Success = false,
      StatusCode = StatusCodes.Status403Forbidden,
      Message = message
    });
  }

  protected IActionResult ToInternalServerError(string message = "An unexpected error occurred")
  {
    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<object>
    {
      Success = false,
      StatusCode = StatusCodes.Status500InternalServerError,
      Message = message
    });
  }
  protected IActionResult ToConflict(string message = "Already exists")
  {
    var response = new ApiResponse<object>
    {
      Success = false,
      StatusCode = StatusCodes.Status409Conflict,
      Message = message
    };
    return Conflict(response);
  }
  #endregion
}
