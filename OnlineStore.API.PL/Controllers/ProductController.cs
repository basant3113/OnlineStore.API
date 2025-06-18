using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.DAL.Models;
using OnlineStore.API.PL.DTOs;
using OnlineStore.API.PL.Errors;
using OnlineStore.API.PL.Helpers;
using System.Security.Claims;

namespace OnlineStore.API.PL.Controllers
{
    public class ProductController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _unitOfWork.productRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(map);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<ProductDTO>> GetById(int Id)
        {
            var product = await _unitOfWork.productRepository.GetByIdAsync(Id);
            if(product is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Product with this Id is not found"));
            var map = _mapper.Map<ProductDTO>(product);
            return Ok(map);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchByName(string Name)
        {
            var products = await _unitOfWork.productRepository.SearchByNameAsync(Name);
            var map = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(map);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> AddProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationResponse(400
           , "a bad Request , You have made"
           , ModelState.Values
           .SelectMany(v => v.Errors)
           .Select(e => e.ErrorMessage)
           .ToList()));


            if (productDTO.Picture is not null)
            {
                productDTO.PictureUrl = DocumentSettings.Upload(productDTO.Picture, "Images");   // خزن الصوره وهات اسمها
            }

            var map = _mapper.Map<Product>(productDTO);

            map.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var count = await _unitOfWork.productRepository.AddAsync(map);
                if (count > 0)
                    return Ok();

            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<int>> UpdateProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationResponse(400
          , "a bad Request , You have made"
          , ModelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage)
          .ToList()));

            var product = await _unitOfWork.productRepository.GetByIdAsync(productDTO.Id);
            if(product is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Product with this Id is not found"));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(product.ApplicationUserId != userId || !User.IsInRole("Admin"))
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Don't have access to remove this Product"));


            if (productDTO.PictureUrl is not null)
                DocumentSettings.Delete(productDTO.PictureUrl, "Images");  //Upload Image to wwwroot


            if (productDTO.Picture is not null)
                productDTO.PictureUrl = DocumentSettings.Upload(productDTO.Picture, "Images");   //Save image && return name


            var map = _mapper.Map<Product>(productDTO);
            map.ApplicationUserId = userId;

            var count = await _unitOfWork.productRepository.UpdateAsync(product);
            if (count > 0)
                return Ok();
            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }


        [HttpDelete]
        public async Task<ActionResult<int>> DeleteProduct(int Id)
        {
            var product = await _unitOfWork.productRepository.GetByIdAsync(Id);
            if (product is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Product with this Id is not found"));

            var count = await _unitOfWork.productRepository.DeleteAsync(product);
            if (count > 0) 
            {
                DocumentSettings.Delete(product.PictureUrl, "Images");  //Upload Image to wwwroot
                return Ok();
            }
            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }
    }
}
