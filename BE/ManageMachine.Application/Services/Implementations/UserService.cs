using AutoMapper;
using ManageMachine.Application.Common;
using ManageMachine.Application.DTOs.Users;
using ManageMachine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, IMapper _mapper)
        {
            _userRepository = userRepository;
            this._mapper = _mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var existing = await _userRepository.GetAsync(u => u.Username == createUserDto.Username);
            if (System.Linq.Enumerable.Any(existing))
            {
                throw new Exception("Username already exists");
            }

            var user = _mapper.Map<User>(createUserDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task ResetPasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            user.FullName = updateUserDto.FullName;
            user.Email = updateUserDto.Email;
            user.Role = updateUserDto.Role;

            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");

            await _userRepository.DeleteAsync(user);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            return _mapper.Map<UserDto>(user);
        }
    }
}
