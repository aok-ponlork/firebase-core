using Firebase_Auth.Common.Filters;
using Firebase_Auth.Data.Constant;
using Firebase_Auth.Data.Models.Authorization;
using Firebase_Auth.Services.Authorization.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Controllers.Authorization;
[Route("api/role")]
[Authorize]
public class RoleController : CoreController
{
    private readonly IRolePermissionService _service;
    public RoleController(IRolePermissionService service, ILogger<RoleController> logger) : base(logger)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> ListAllRoleAsync([FromQuery] SimpleFilter filter)
    {
        var result = await _service.GetAllRolesAsync(filter);
        return ToSuccess("Listed Roles.", result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync([FromBody] RoleCreateDto role)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ToBadRequest(ModelState);
            }
            await _service.CreateRoleAsync(role);
            return ToCreated("");
        }
        catch (InvalidOperationException ex)
        {
            return ToConflict(ex.Message);
        }
        catch (Exception)
        {
            return ToInternalServerError("An error occurred during authentication.");
        }
    }
}