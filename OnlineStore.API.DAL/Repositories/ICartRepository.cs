using OnlineStore.API.Core.Models;
using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetByUserIdAsync(string userId);
        Task<Cart> GetWithItemsByUserIdAsync(string userId);
        Task<Cart> GetCartById(int Id);
        Task<int> AddAsync(Cart cart);
        Task<int> AddProductToCart(Cart cart, int ProductId, int Quantity);
        Task<int> RemoveProductFromCart(Cart cart, int productId);
        Task<int> Update(Cart cart);
        Task<int> Remove(Cart cart);
        Task<int> ClearCart(Cart cart);

    }
}
