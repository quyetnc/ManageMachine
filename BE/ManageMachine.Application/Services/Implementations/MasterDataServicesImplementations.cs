using AutoMapper;
using ManageMachine.Application.Common;
using ManageMachine.Application.DTOs.Machine;
using ManageMachine.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services.Implementations
{
    public class MachineTypeService : IMachineTypeService
    {
        private readonly IGenericRepository<MachineType> _repository;
        private readonly IMapper _mapper;

        public MachineTypeService(IGenericRepository<MachineType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MachineTypeDto> CreateAsync(CreateMachineTypeDto createDto)
        {
            var entity = _mapper.Map<MachineType>(createDto);
            await _repository.AddAsync(entity);
            return _mapper.Map<MachineTypeDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null) await _repository.DeleteAsync(entity);
        }

        public async Task<IReadOnlyList<MachineTypeDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<MachineTypeDto>>(list);
        }

        public async Task<MachineTypeDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<MachineTypeDto>(entity);
        }

        public async Task UpdateAsync(int id, CreateMachineTypeDto updateDto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _mapper.Map(updateDto, entity);
                await _repository.UpdateAsync(entity);
            }
        }
    }

    public class ParameterService : IParameterService
    {
        private readonly IGenericRepository<Parameter> _repository;
        private readonly IMapper _mapper;

        public ParameterService(IGenericRepository<Parameter> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ParameterDto> CreateAsync(CreateParameterDto createDto)
        {
            var entity = _mapper.Map<Parameter>(createDto);
            await _repository.AddAsync(entity);
            return _mapper.Map<ParameterDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null) await _repository.DeleteAsync(entity);
        }

        public async Task<IReadOnlyList<ParameterDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ParameterDto>>(list);
        }

        public async Task<ParameterDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<ParameterDto>(entity);
        }

        public async Task UpdateAsync(int id, CreateParameterDto updateDto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _mapper.Map(updateDto, entity);
                await _repository.UpdateAsync(entity);
            }
        }
    }
}
