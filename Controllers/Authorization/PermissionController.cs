using Firebase_Auth.Common;
using Firebase_Auth.Data.Constant;
using Firebase_Auth.Services.Authorization.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Controllers.Authorization;
[Route("api/permission")]
[Authorize]
public class PermissionController : CoreController
{
    private readonly IRolePermissionService _service;
    public PermissionController(IRolePermissionService service, ILogger<PermissionController> logger) : base(logger)
    {
        _service = service;
    }
    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet]
    public async Task<IActionResult> ListAllRoleAsync([FromQuery] PaginationFilter filter)
    {
        var result = await _service.GetAllPermissionsAsync(filter);
        return ToSuccess("Listed Permission.", result);
    }
}