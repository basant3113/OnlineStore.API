using OnlineStore.API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Repositories
{
    public interface ICommentRepository
    {
        public Task<IEnumerable<Comment>> GetAllCommentsForProductAsync(int productId);
        public Task<Comment> GetByIdAsync(int id);
        public Task<int> AddCommentAsync(Comment comment);
        public Task<int> UpdateCommentAsync(Comment comment);
        public Task<int> DeleteCommentAsync(Comment comment);

    }
}
