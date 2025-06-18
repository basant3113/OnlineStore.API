using OnlineStore.API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Repositories
{
    public interface IFavouriteRepository
    {
        Task<int> AddAsync(Favourite fav);
        Task<IReadOnlyList<Favourite>> GetFavouritesByUserIdAsync(string userId);
        Task<Favourite> GetFavouritesAsync(string userId, int productId);
        Task<int> DeleteFavouritesAsync(Favourite FavouriteId);
    }
}
