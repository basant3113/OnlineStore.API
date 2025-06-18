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
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _Context;
        private IGenaricRepository<Product> _productRepository;
        private IGenaricRepository<ProductType> _typeRepository;
        private IGenaricRepository<ProductType> _brandRepository;
        private ICommentRepository _commentRepository;
        private IFavouriteRepository _favouriteRepository;
        private ICartRepository _cartRepository;
        public UnitOfWork(ApplicationDbContext Context)
        {
            _Context = Context;
            _productRepository = new ProductRepository(_Context);
            _typeRepository = new TypeRepository(_Context);
            _brandRepository = new TypeRepository(_Context);
            _commentRepository = new CommentRepository(_Context);
            _favouriteRepository = new FavouriteRepository(_Context);
            _cartRepository = new CartRepository(_Context);
        }

        public IGenaricRepository<Product> productRepository => _productRepository;

        public IGenaricRepository<ProductType> brandRepository => _brandRepository;

        public IGenaricRepository<ProductType> typeRepository => _typeRepository;
        public ICommentRepository commentRepository => _commentRepository;
        public IFavouriteRepository favouriteRepository => _favouriteRepository;

        public ICartRepository cartRepository => _cartRepository;
    }
}
