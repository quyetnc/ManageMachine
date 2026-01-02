using AutoMapper;
using ManageMachine.Application.Common;
using ManageMachine.Application.DTOs.Auth;
using ManageMachine.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthService(IGenericRepository<User> userRepository, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var users = await _userRepository.GetAsync(u => u.Username == loginDto.Username);
            var user = users.FirstOrDefault();

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            var response = _mapper.Map<AuthResponseDto>(user);
            response.Token = token;

            return response;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existing = await _userRepository.GetAsync(u => u.Username == registerDto.Username);
            if (existing.Any())
            {
                throw new Exception("Username already exists");
            }

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            
            await _userRepository.AddAsync(user);

            var token = _jwtTokenGenerator.GenerateToken(user);
            var response = _mapper.Map<AuthResponseDto>(user);
            response.Token = token;

            return response;
        }
    }
}
