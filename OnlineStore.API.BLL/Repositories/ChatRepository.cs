using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineStore.API.BLL.Data.Contexts;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.Repository.RealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Repository.Repositories
{
    public class ChatRepository:IChatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatRepository(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<int> SendMessageAsync(ChatMessage chatMessage)
        {
            await _context.ChatMessages.AddAsync(chatMessage);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string receiverId, string senderId)
            => await _context.ChatMessages.Where(e => (e.SenderId == senderId && e.ReceiverId == receiverId) ||
                                                      (e.SenderId == receiverId && e.ReceiverId == senderId))
                                                      .OrderBy(t => t.Timestamp)
                                                      .ToListAsync();

        public async Task<int> DeleteAsync(ChatMessage chatMessage)
        {
            _context.ChatMessages.Remove(chatMessage);
            return await _context.SaveChangesAsync();
        }

        public async Task<ChatMessage> GetMessageAsync(int MessageId)
            => await _context.ChatMessages.FindAsync(MessageId);
    }
}
