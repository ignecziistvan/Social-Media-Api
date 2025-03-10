using API.Dtos.Request;
using API.Dtos.Response;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MessageController(IMessageRepository messageRepository, 
    IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(SendMessageDto dto)
    {
        string username = User.GetUsername();
        if (username == dto.ReceiverUsername.ToLower())
            return BadRequest("You cannot message yourself");
        
        var sender = await userRepository.GetUserByUserNameAsNonDto(username);
        var receiver = await userRepository.GetUserByUserNameAsNonDto(dto.ReceiverUsername);

        if (receiver == null || sender == null || sender.UserName == null || receiver.UserName == null) 
            return BadRequest("Cannot send message");

        Message message = new Message
        {
            Sender = sender,
            Receiver = receiver,
            SenderUsername = sender.UserName,
            ReceiverUsername = receiver.UserName,
            Text = dto.Text
        };

        messageRepository.AddMessage(message);

        if (await messageRepository.Complete()) return Ok(mapper.Map<MessageDto>(message));

        return BadRequest("Failed to save message");
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<MessageDto>>> GetMessagesForUser(
        [FromQuery] MessageParams messageParams
    )
    {
        messageParams.Username = User.GetUsername();

        var messages = await messageRepository.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(messages);

        return Ok(messages);
    }

    [Authorize]
    [HttpGet("thread/{username}")]
    public async Task<ActionResult<List<MessageDto>>> GetMessageThread(string username)
    {
        string currentUsername = User.GetUsername();

        return Ok(await messageRepository.GetMessageThread(currentUsername, username));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        string username = User.GetUsername();

        Message? message = await messageRepository.GetMessageById(id);
        if (message == null) return BadRequest("Could not find message");

        if (message.SenderUsername != username && message.ReceiverUsername != username) 
            return Forbid();

        if (message.SenderUsername == username) message.SenderDeleted = true;

        if (message.ReceiverUsername == username) message.ReceiverDeleted = true;

        if (message is {SenderDeleted: true, ReceiverDeleted: true}) {
            messageRepository.DeleteMessage(message);
        }

        if (await messageRepository.Complete()) return Ok();

        return BadRequest("Failed to delete message");
    }
}
