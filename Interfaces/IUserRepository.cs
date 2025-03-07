using API.Dtos.Response;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetUser(string username);
    Task<User?> GetUserByIdAsNonDto(int id);
    Task<User?> GetUserByUserNameAsNonDto(string username);
    Task<IEnumerable<UserDto>> GetAllUsers();
    Task<bool> Complete();
}
