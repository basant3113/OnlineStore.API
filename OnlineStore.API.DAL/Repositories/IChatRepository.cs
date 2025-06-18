using OnlineStore.API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Repositories
{
    public interface IChatRepository
    {
        Task<List<ChatMessage>> GetChatHistoryAsync(string receiverId, string senderId);
        Task<ChatMessage> GetMessageAsync(int MessageId);
        Task<int> SendMessageAsync(ChatMessage chatMessage);
        Task<int> DeleteAsync(ChatMessage chatMessage);
    }
}
