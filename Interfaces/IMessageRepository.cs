using API.Dtos.Response;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message?> GetMessageById(int id);
    Task<List<MessageDto>> GetMessagesForUser(string username, string? container);
    Task<List<MessageDto>> GetMessageThread(string currentUsername, string receiverUsername);
    Task<bool> Complete();
}