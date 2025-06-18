using Microsoft.EntityFrameworkCore;
using OnlineStore.API.BLL.Data.Contexts;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Repository.Repositories
{
    public class CommentRepository:ICommentRepository
    {
        private readonly ApplicationDbContext _Context;

        public CommentRepository(ApplicationDbContext context)
        {
            _Context = context;
        }
        public async Task<int> AddCommentAsync(Comment comment)
        {
            await _Context.Comments.AddAsync(comment);
            return await _Context.SaveChangesAsync();
        }

        public async Task<int> UpdateCommentAsync(Comment comment)
        {
            _Context.Comments.Update(comment);
            return await _Context.SaveChangesAsync();
        }

        public async Task<int> DeleteCommentAsync(Comment comment)
        {
            _Context.Comments.Remove(comment);
            return await _Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsForProductAsync(int productId)
        {
            return await _Context.Comments.Where(e => e.productId == productId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _Context.Comments.Include(c => c.User).FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
