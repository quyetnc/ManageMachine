using ManageMachine.Application.DTOs.Machine;
using ManageMachine.Application.DTOs.Requests;
using ManageMachine.Application.Services;
using ManageMachine.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;


namespace ManageMachine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MachinesController : ControllerBase
    {
        private readonly IMachineService _service;

        public MachinesController(IMachineService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MachineDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("mine")]
        public async Task<ActionResult<IReadOnlyList<MachineDto>>> GetMyMachines()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();
            
            if (int.TryParse(userIdClaim, out int userId))
            {
                 return Ok(await _service.GetByUserIdAsync(userId));
            }
            return BadRequest("Invalid User Id in token");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MachineDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpGet("qr/{code}")]
        public async Task<ActionResult<MachineDto>> GetByQRCode(string code)
        {
            // Allow even normal users to scan
            var item = await _service.GetByQRCodeAsync(code);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MachineDto>> Create(CreateMachineDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, CreateMachineDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/parameters")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddParameter(int id, CreateMachineParameterDto paramDto)
        {
            await _service.AddParameterToMachineAsync(id, paramDto);
            return Ok();
        }
        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnMachine(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _service.ReturnMachineAsync(id, userId);
            return Ok();
        }

        [HttpGet("{id}/history")]
        public async Task<ActionResult<IReadOnlyList<MachineTransferRequestDto>>> GetHistory(int id)
        {
            var history = await _service.GetHistoryAsync(id);
            return Ok(history);
        }
    }
}
