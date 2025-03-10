using API.Dtos.Response;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByUsername(string username);
    Task<User?> GetUserById(int id);
    Task<PaginatedList<UserDto>> GetAllUsers(PaginationParams paginationParams);
    Task<bool> Complete();
}
