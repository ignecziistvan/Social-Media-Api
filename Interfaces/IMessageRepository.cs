using API.Dtos.Response;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message?> GetMessageById(int id);
    Task<PaginatedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
    Task<List<MessageDto>> GetMessageThread(string currentUsername, string receiverUsername);
    Task<bool> Complete();
}