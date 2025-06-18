using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.API.Core.Models;
using OnlineStore.API.Core.Services;
using OnlineStore.API.PL.DTOs;
using OnlineStore.API.PL.Errors;

namespace OnlineStore.API.PL.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ITokenService _TokenService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IMapper mapper,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _TokenService = tokenService;
        }


        [Authorize]
        [HttpGet("GetAllUsers")]
        public ActionResult GetAllUsers()
        {
            var users = _userManager.Users.Select(e => new
            {
                e.Id,
                e.UserName,
                e.Email
            }).ToList();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("GetUserById")]
        public async Task<ActionResult> GetUserById(string userId)
        {
             var user = await _userManager.FindByIdAsync(userId);
             if (user is not null)
             {
                 return Ok(user);
             }

            return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "User with this Id is not found"));
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(registerDTO.Email);
                if (user is not null)
                    return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "User with this Email is found"));


                    var appUser = _mapper.Map<ApplicationUser>(registerDTO);

                    var result = await _userManager.CreateAsync(appUser, registerDTO.Password);    //Create Account
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(appUser, $"{registerDTO.Role}");

                        var ReturnedUser = new UserDTO()
                        {

                            Id = appUser.Id,
                            role = "User",
                            UserName = appUser.UserName,
                            Token = await _TokenService.CreateTokenAsync(appUser, _userManager)
                        };
                        return Ok(ReturnedUser);
                    }

                return BadRequest(new ApiValidationResponse(StatusCodes.Status400BadRequest
                           , "a bad Request , You have made"
                           , result.Errors.Select(e => e.Description).ToList()));
            }

            return BadRequest(new ApiValidationResponse(400
                       , "a bad Request , You have made"
                       , ModelState.Values
                       .SelectMany(v => v.Errors)
                       .Select(e => e.ErrorMessage)
                       .ToList()));
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, loginDTO.Password))
                    {
                        var userDTO = new UserDTO()
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            role = "User",
                            Token = await _TokenService.CreateTokenAsync(user, _userManager)
                        };
                        var checkTrainer = await _userManager.IsInRoleAsync(user, "Admin");
                        if (checkTrainer)
                            userDTO.role = "Admin";
                        return Ok(userDTO);
                    }
                    return NotFound(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Password with this Email InCorrect"));
                }
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "User with this Email is not found"));
            }
            return BadRequest(new ApiValidationResponse(400
                       , "a bad Request , You have made"
                       , ModelState.Values
                       .SelectMany(v => v.Errors)
                       .Select(e => e.ErrorMessage)
                       .ToList()));
        }


        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(registerDTO.Id);

                if (user is not null)
                {

                    user.PhoneNumber = registerDTO.PhoneNumber;
                    user.Email = registerDTO.Email;
                    user.UserName = registerDTO.UserName;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok("Updated");
                    }
                    return BadRequest(new ApiValidationResponse(StatusCodes.Status400BadRequest
                             , "a bad Request , You have made"
                             , result.Errors.Select(e => e.Description).ToList()));
                }

                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "User with this Id is not found"));
            }
            return BadRequest(new ApiValidationResponse(400
                      , "a bad Request , You have made"
                      , ModelState.Values
                      .SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)
                      .ToList()));

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is not null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                    return BadRequest(new ApiValidationResponse(StatusCodes.Status400BadRequest
                , "a bad Request , You have made"
                , result.Errors.Select(e => e.Description).ToList()));
                }
                return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound, "User with this Id is not found"));
            }

            return BadRequest(new ApiValidationResponse(400
                      , "a bad Request , You have made"
                      , ModelState.Values
                      .SelectMany(v => v.Errors)
                      .Select(e => e.ErrorMessage)
                      .ToList()));
        }
    }
}
