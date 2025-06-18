using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.PL.DTOs;
using OnlineStore.API.PL.Errors;
using System.Security.Claims;

namespace OnlineStore.API.PL.Controllers
{
    public class FavouriteController : BaseAPIController
    {
        private readonly IMapper _Mapper;
        private readonly IUnitOfWork _unitOfWork;

        public FavouriteController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _Mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favourite>>> GetUserFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _unitOfWork.favouriteRepository.GetFavouritesByUserIdAsync(userId);
            return Ok(favorites);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddToFavorites([FromBody] FavouriteDTO favoriteDTO)
        {
            if (ModelState.IsValid)
            {
                var apartment = await _unitOfWork.productRepository.GetByIdAsync(favoriteDTO.ProductId);
                if (apartment is null)
                    return NotFound(new ApiErrorResponse(404, "Product with this id is not found"));

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var fav = await _unitOfWork.favouriteRepository.GetFavouritesAsync(userId, favoriteDTO.ProductId);
                if (fav is not null)
                    return BadRequest(new ApiErrorResponse(400, "This Product is added to this user before"));



                var map = _Mapper.Map<Favourite>(favoriteDTO);
                map.UserId = userId;
                var count = await _unitOfWork.favouriteRepository.AddAsync(map);
                if (count > 0)
                {
                    return Ok(favoriteDTO);
                }
                return BadRequest(new ApiErrorResponse(400, "Error in save favourite"));
            }
            return BadRequest(new ApiValidationResponse(StatusCodes.Status400BadRequest
                    , "a bad Request , You have made"
                    , ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        }


        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> RemoveFromFavorites( int ProductId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fav = await _unitOfWork.favouriteRepository.GetFavouritesAsync(userId, ProductId);
            if (fav is null)
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "User don't have this product in her favourite"));

                var count = await _unitOfWork.favouriteRepository.DeleteFavouritesAsync(fav);
                if (count > 0)
                {
                    return Ok();
                }
                return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error when remove favourite"));
        }
    }
}
