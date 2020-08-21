using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnonymousWebApi.Data.DomainModel;
using AnonymousWebApi.Data.EFCore.Repository;
using AnonymousWebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;
        private UserAddressRepository _userAddressRepository;

        
        public UserProfileController(UserManager<ApplicationUser> userManager,
            UserAddressRepository userAddressRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userAddressRepository = userAddressRepository;
        }

        [HttpGet]
        [Authorize]
        //GET : /api/UserProfile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            return new
            {
                user.FullName,
                user.Email,
                user.UserName
            };
        }

        [Produces("application/json")]
        [HttpGet]
        [Authorize]
        [Route("GetApplicationUser")]
        public async Task<IActionResult> GetApplicationUser()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            
            return Ok(_mapper.Map<ApplicationUserModel>(user));
        }

        [HttpPost]
        [Authorize]
        [Route("PostUserAddress")]
        public async Task<IActionResult> AddUserAddress(UserAddressModel model)
        {
            return Ok(await _userAddressRepository.Add(_mapper.Map<UserAddressModel, UserAddress>(model)).ConfigureAwait(false));
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserAddresses")]
        public async Task<IActionResult> GetUserAddresses()
        {
            var result = await _userAddressRepository.GetAll().ConfigureAwait(false);
            
           
            var data = _mapper.Map<IEnumerable<UserAddress>,IEnumerable<UserAddressModel>>(result);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("ForAdmin")]
        public string GetForAdmin()
        {
            return "Web method for Admin";
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("ForCustomer")]
        public string GetCustomer()
        {
            return "Web method for Customer";
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        [Route("ForAdminOrCustomer")]
        public string GetForAdminOrCustomer()
        {
            return "Web method for Admin or Customer";
        }
    }
}