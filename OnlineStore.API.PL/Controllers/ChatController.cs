using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.PL.DTOs;
using OnlineStore.API.PL.Errors;
using OnlineStore.API.Repository.RealTime;
using System.Security.Claims;

namespace OnlineStore.API.PL.Controllers
{
    [Authorize]
    public class ChatController : BaseAPIController
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IChatRepository _ChatRepository;

        public ChatController(IHubContext<ChatHub> hubContext, IChatRepository chatRepository)
        {
            _hubContext = hubContext;
            _ChatRepository = chatRepository;
        }

        // Send a message via HTTP API
        [Authorize]
        [HttpPost("send")]
        public async Task<ActionResult> SendMessage([FromBody] SendMessageDTO request)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user ID

            if (senderId is null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid to get sender Id please sure a sign in "));

            var chatMessage = new ChatMessage   //Mapping
            {
                SenderId = senderId,
                ReceiverId = request.ReceiverId,
                Message = request.Message,
                Timestamp = DateTime.UtcNow
            };

            var count = await _ChatRepository.SendMessageAsync(chatMessage);

            if (count > 0)
            {
                // Send message via SignalR if receiver is online
                await _hubContext.Clients.User(request.ReceiverId).SendAsync("ReceiveMessage", senderId, request.Message);
                return Ok(chatMessage);
            }
            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save message please try again"));
        }

        [Authorize]
        [HttpGet("GetChat")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetChatHistory(string receiverId)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderId is null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Invalid to get sender Id please sure a sign in "));

            var messages = await _ChatRepository.GetChatHistoryAsync(receiverId, senderId);

            return Ok(messages);
        }


        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteMessage(int MessageId)
        {
            var message = await _ChatRepository.GetMessageAsync(MessageId);
            if (message is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Message with this Id is not found"));

            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderId != message.SenderId)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Don't have an access to remove this message"));

            var count = await _ChatRepository.DeleteAsync(message);
            if (count > 0)
            {

                // Notify the sender and receiver in real-time
                await _hubContext.Clients.Users(message.SenderId, message.ReceiverId)
                    .SendAsync("MessageDeleted", MessageId);
                return Ok();
            }
            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in delete please try again"));
        }
    }
}
