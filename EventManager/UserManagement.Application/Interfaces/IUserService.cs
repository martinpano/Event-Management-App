using UserManagement.Application.DTOs;

namespace UserManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(RegisterUserDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);
    }
}
