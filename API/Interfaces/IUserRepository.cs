using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
