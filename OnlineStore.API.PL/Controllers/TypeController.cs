using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Core.Repositories;
using OnlineStore.API.DAL.Models;
using OnlineStore.API.PL.DTOs;
using OnlineStore.API.PL.Errors;

namespace OnlineStore.API.PL.Controllers
{
    public class TypeController :BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAll()
        {
            var types = await _unitOfWork.typeRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<TypeDTO>>(types);
            return Ok(map);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<TypeDTO>> GetById(int Id)
        {
            var Type = await _unitOfWork.typeRepository.GetByIdAsync(Id);
            if (Type is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Type with this Id is not found"));

            var map = _mapper.Map<TypeDTO>(Type);
            return Ok(map);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> SearchByName(string Name)
        {
            var types = await _unitOfWork.typeRepository.SearchByNameAsync(Name);
            var map = _mapper.Map<IEnumerable<TypeDTO>>(types);
            return Ok(map);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddBrand(TypeDTO typeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationResponse(400
          , "a bad Request , You have made"
          , ModelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage)
          .ToList()));

          var map = _mapper.Map<ProductType>(typeDTO);
          var count = await _unitOfWork.typeRepository.AddAsync(map);
          if (count > 0)
              return Ok();
          return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }

        [HttpPut]
        public async Task<ActionResult<int>> UpdateBrand(TypeDTO typeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationResponse(400
          , "a bad Request , You have made"
          , ModelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage)
          .ToList()));

          var map = _mapper.Map<ProductType>(typeDTO);
          var count = await _unitOfWork.typeRepository.UpdateAsync(map);
          if (count > 0)
              return Ok();
            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }


        [HttpDelete]
        public async Task<ActionResult<int>> DeleteBrand(int Id)
        {
            var type = await _unitOfWork.typeRepository.GetByIdAsync(Id);
            if (type is null)
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "Type with this Id is not found"));

            var count = await _unitOfWork.typeRepository.DeleteAsync(type);
            if (count > 0)
                return Ok();
            return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Error in save , please try again"));
        }
    }
}
