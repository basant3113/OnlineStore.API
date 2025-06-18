using Microsoft.EntityFrameworkCore;
using OnlineStore.API.BLL.Data.Contexts;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Repositories;

namespace OnlineStore.API.Repository.Repositories
{
    public class FavouriteRepository:IFavouriteRepository
    {
        private readonly ApplicationDbContext _Context;

        public FavouriteRepository(ApplicationDbContext context)
        {
            _Context = context;
        }
        public async Task<int> AddAsync(Favourite fav)
        {
            await _Context.Favourites.AddAsync(fav);
            return await _Context.SaveChangesAsync();
        }

        public async Task<int> DeleteFavouritesAsync(Favourite fav)
        {
            _Context.Favourites.Remove(fav);
            return await _Context.SaveChangesAsync();
        }

        public async Task<Favourite> GetFavouritesAsync(string userId, int productId)
        {
            return await _Context.Favourites
                            .Where(F => F.UserId == userId && F.productId == productId)
                            .FirstOrDefaultAsync();

        }

        public async Task<IReadOnlyList<Favourite>> GetFavouritesByUserIdAsync(string userId)
        {
            return await _Context.Favourites
                             .Where(F => F.UserId == userId)
                             .Include(P => P.product)
                             .ToListAsync();

        }
    }
}
