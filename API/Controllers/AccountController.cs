using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController (UserManager<AppUser> userManager, SignInManager<AppUser>
         signInManager,ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;

        }
        [Authorize]
        [HttpGet]

        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindEmailByClaimsPrinciple(User);
             return new UserDto{
                    DisplayName = user.DisplayName,
                    Token = _tokenService.CreateToken(user),
                    Email = user.Email
                };
        }
        
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;

        }
        
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(User);
            return _mapper.Map<Address , AddressDto>(user.Address);

        }
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);
 
            user.Address = _mapper.Map<AddressDto,Address>(address);

            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDto>(user.Address));
            return BadRequest("problem updating user");

        }
        
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(string email,string password,LoginDto loginDto)
        {
           var user = await _userManager.FindByEmailAsync(loginDto.Email);
           // var userr = await _userManager.FindByEmailAsync(email) != null;

           if(user == null) return Unauthorized(new ApiResponse(401));
            
             var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,
             false);

             if(!result.Succeeded) return Unauthorized(new ApiResponse(401));

             return new UserDto
             {
                 Email = loginDto.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
             };

        }
            [HttpPost("register")]
            public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
            {
                if(CheckEmailExistsAsync(registerDto.Email).Result.Value)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"آدرس ایمیل تکراری می باشد"}});
                }
                var user = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if(!result.Succeeded) return BadRequest(new ApiResponse(400));

                return new UserDto{
                    DisplayName = user.DisplayName,
                    Token = _tokenService.CreateToken(user),
                    Email = user.Email
                };
            }
    }
}