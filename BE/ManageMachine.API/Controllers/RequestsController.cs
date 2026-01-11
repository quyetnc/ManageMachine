using ManageMachine.Application.DTOs.Requests;
using ManageMachine.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ManageMachine.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMachineTransferRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var request = await _requestService.CreateRequestAsync(dto, userId);
            return Ok(request);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPending()
        {
            var requests = await _requestService.GetPendingRequestsAsync();
            return Ok(requests);
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            await _requestService.ApproveRequestAsync(id);
            return Ok();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            await _requestService.RejectRequestAsync(id);
            return Ok();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _requestService.CancelRequestAsync(id, userId);
            return Ok();
        }

        [HttpGet("my-requests")]
        public async Task<ActionResult<IReadOnlyList<MachineTransferRequestDto>>> GetMyRequests()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            return Ok(await _requestService.GetRequestsByUserIdAsync(userId));
        }
    }
}
