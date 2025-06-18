using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.PL.DTOs.CartDto;
using OnlineStore.API.PL.Errors;
using System.Security.Claims;

namespace OnlineStore.API.PL.Controllers
{
    public class CartController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("GetUserCart")]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user ID
            var cart = await _unitOfWork.cartRepository.GetWithItemsByUserIdAsync(userId);
            if (cart is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Cart with this UserId is not found"));

            var map = new CartDto
            {
                Id = cart.Id,
                ApplicationUserId = cart.ApplicationUserId,
                TotalAmount = cart.CartItems.Sum(e => e.Product.NewPrice * e.Quantity),
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name, // تأكد من تضمين Product في Include
                    ProductPictureUrl = $"{Request.Scheme}://{Request.Host}/Images/" + item.Product.PictureUrl,
                    Quantity = item.Quantity,
                    Price = item.Product.NewPrice * item.Quantity,
                }).ToList()
            };

            return Ok(map);
        }

        [Authorize]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(AddCartDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationResponse(400
                       , "a bad Request , You have made"
                       , ModelState.Values
                       .SelectMany(v => v.Errors)
                       .Select(e => e.ErrorMessage)
                       .ToList()));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user ID
            var product = await _unitOfWork.productRepository.GetByIdAsync(cartDto.ProductId);
            if (product is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Product with this Id is not found"));

            var cart = await _unitOfWork.cartRepository.GetWithItemsByUserIdAsync(userId);

            if (cart is null)
            {
                cart = new Cart
                {
                    ApplicationUserId = userId,
                    CartItems = new List<CartItem>()
                };
                var count = await _unitOfWork.cartRepository.AddAsync(cart);
                if (count == 0)
                    return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
            }
            var count2 = await _unitOfWork.cartRepository.AddProductToCart(cart, cartDto.ProductId, cartDto.Quantity);
            if (count2 > 0)
                return Ok();

            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }

        [Authorize]
        [HttpDelete("RemoveFromCart")]
        public async Task<IActionResult> RemoveFromCart(string userId, int productId)
        {
            var cart = await _unitOfWork.cartRepository.GetWithItemsByUserIdAsync(userId);
            if (cart is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Cart with this UserId is not found"));

            var result = await _unitOfWork.cartRepository.RemoveProductFromCart(cart, productId);
            if (result > 0)
                return Ok();

            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }

        [Authorize]
        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var cart = await _unitOfWork.cartRepository.GetWithItemsByUserIdAsync(userId);
            if (cart is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Cart with this UserId is not found"));

            var count = await _unitOfWork.cartRepository.ClearCart(cart);
            if (count > 0)
                return Ok();

            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }
    }
}
