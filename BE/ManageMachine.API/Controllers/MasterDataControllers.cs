using ManageMachine.Application.DTOs.Machine;
using ManageMachine.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageMachine.API.Controllers
{
    [ApiController]
    [Route("api/types")]
    [Authorize(Roles = "Admin")] // Only admin manages types
    public class MachineTypesController : ControllerBase
    {
        private readonly IMachineTypeService _service;

        public MachineTypesController(IMachineTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous] // Maybe users can see types? Or just authenticated users. Let's say Auth users.
        public async Task<ActionResult<IReadOnlyList<MachineTypeDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MachineTypeDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<MachineTypeDto>> Create(CreateMachineTypeDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CreateMachineTypeDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

    [ApiController]
    [Route("api/parameters")]
    [Authorize(Roles = "Admin")]
    public class ParametersController : ControllerBase
    {
        private readonly IParameterService _service;

        public ParametersController(IParameterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ParameterDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParameterDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ParameterDto>> Create(CreateParameterDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CreateParameterDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
