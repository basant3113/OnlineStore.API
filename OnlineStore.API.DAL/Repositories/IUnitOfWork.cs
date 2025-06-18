using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.Core.Repositories
{
    public interface IUnitOfWork
    {
        public IGenaricRepository<Product> productRepository { get; }
        public IGenaricRepository<ProductType> brandRepository { get; }
        public IGenaricRepository<ProductType> typeRepository { get; }
        public ICommentRepository commentRepository { get; }
        public ICartRepository cartRepository { get; }
        public IFavouriteRepository favouriteRepository { get; }
    }
}