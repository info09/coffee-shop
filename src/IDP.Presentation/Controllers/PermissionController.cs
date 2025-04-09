using IDP.Infrastructure.Repositories;
using IDP.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace IDP.Presentation.Controllers
{
    [Route("api/roles/{roleId}/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;

        public PermissionController(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissionsByRole(string roleId)
        {
            var permissions = await _repositoryManager.Permission.GetPermissionsByRole(roleId);
            return Ok(permissions);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission(string roleId, [FromBody] PermissionAddModel model)
        {
            var result = await _repositoryManager.Permission.CreatePermission(roleId, model);
            return result != null ? Ok(result) : BadRequest("Failed to create permission");
        }

        [HttpDelete("function/{function}/command/{command}")]
        [ProducesResponseType(typeof(PermissionViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeletePermission(string roleId, [Required] string function, [Required] string command)
        {
            await _repositoryManager.Permission.DeletePermission(roleId, function, command);
            return NoContent();
        }

        [HttpPost("update-permissions")]
        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdatePermissions(string roleId, [FromBody] IEnumerable<PermissionAddModel> permissions)
        {
            await _repositoryManager.Permission.UpdatePermissionsByRoleId(roleId, permissions);
            return NoContent();
        }
    }
}
