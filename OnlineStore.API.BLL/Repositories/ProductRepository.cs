using Microsoft.EntityFrameworkCore;
using OnlineStore.API.BLL.Data.Contexts;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Repository.Repositories
{
    public class ProductRepository : IGenaricRepository<Product>
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<IEnumerable<Product>> SearchByNameAsync(string Name)
        {
            return await _context.Products.Where(e=>e.Name.Contains(Name)).ToListAsync();
        }
        public async Task<int> AddAsync(Product Entity)
        {
            await _context.Products.AddAsync(Entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(Product Entity)
        {
            _context.Products.Update(Entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Product Entity)
        {
             _context.Products.Remove(Entity);
            return await _context.SaveChangesAsync();
        }
    }
}
