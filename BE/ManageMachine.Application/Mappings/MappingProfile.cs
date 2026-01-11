using AutoMapper;
using ManageMachine.Application.DTOs.Auth;
using ManageMachine.Application.DTOs.Machine;
using ManageMachine.Domain.Entities;

namespace ManageMachine.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Auth
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<User, AuthResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            // Machine
            CreateMap<MachineType, MachineTypeDto>().ReverseMap();
            CreateMap<MachineType, CreateMachineTypeDto>().ReverseMap();

            CreateMap<Parameter, ParameterDto>().ReverseMap();
            CreateMap<Parameter, CreateParameterDto>().ReverseMap();

            CreateMap<MachineParameter, MachineParameterDto>()
                .ForMember(dest => dest.ParameterName, opt => opt.MapFrom(src => src.Parameter.Name))
                .ForMember(dest => dest.ParameterUnit, opt => opt.MapFrom(src => src.Parameter.Unit));
            CreateMap<CreateMachineParameterDto, MachineParameter>();

            CreateMap<Machine, MachineDto>()
                .ForMember(dest => dest.MachineTypeName, opt => opt.MapFrom(src => src.MachineType.Name))
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
                .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.FullName : null))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PendingTransferRequestId, opt => opt.MapFrom(src => src.TransferRequests
                    .Where(r => r.Status == ManageMachine.Domain.Enums.RequestStatus.Pending)
                    .Select(r => (int?)r.Id)
                    .FirstOrDefault()));
            CreateMap<CreateMachineDto, Machine>();
        }
    }
}
