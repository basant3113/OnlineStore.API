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
    public class TypeRepository:IGenaricRepository<ProductType>
    {
        private readonly ApplicationDbContext _context;

        public TypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductType>> GetAllAsync()
        {
            return await _context.Types.ToListAsync();

        }
        public async Task<ProductType> GetByIdAsync(int id)
        {
            return await _context.Types.FindAsync(id);
        }
        public async Task<IEnumerable<ProductType>> SearchByNameAsync(string Name)
        {
            return await _context.Types.Where(e => e.Name.Contains(Name)).ToListAsync();
        }
        public async Task<int> AddAsync(ProductType Entity)
        {
            await _context.Types.AddAsync(Entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(ProductType Entity)
        {
            _context.Types.Update(Entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(ProductType Entity)
        {
            _context.Types.Remove(Entity);
            return await _context.SaveChangesAsync();
        }
    }
}
