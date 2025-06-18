using Microsoft.EntityFrameworkCore;
using OnlineStore.API.BLL.Data.Contexts;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Repository.Repositories
{
    public class CartRepository:ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetByUserIdAsync(string userId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
        }

        public async Task<Cart> GetWithItemsByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);
        }

        public async Task<Cart> GetCartById(int Id)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == Id);
            return cart;
        }

        public async Task<int> AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddProductToCart(Cart cart, int productId, int quantity)
        {
            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem is not null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveProductFromCart(Cart cart, int productId)
        {
            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem is not null)
            {
                cart.CartItems.Remove(existingItem);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Cart cart)
        {
            _context.Carts.Update(cart);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Cart cart)
        {
            _context.Carts.Remove(cart);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ClearCart(Cart cart)
        {
            cart.CartItems.Clear();
            return await _context.SaveChangesAsync();
        }

    }
}

