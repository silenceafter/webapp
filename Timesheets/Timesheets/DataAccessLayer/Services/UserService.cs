using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;
using Timesheets.DataAccessLayer.Repositories.Interfaces;
using Timesheets.DataAccessLayer.Services.Interfaces;

namespace Timesheets.DataAccessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private IUserRepository _userRepository;

        public UserService(          
            ILogger<UserService> logger,
            IUserRepository userRepository
            )
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public UserModel RegisterUser(UserRequest user)
        {
            _logger.LogInformation("RegisterUser() запуск метода");
            var newId = _userRepository.RegisterUser(new UserDto()
            {
                Login = user.Login,
                Password = user.Password,
                RoleId = user.RoleId
            });

            if (newId > 0)
            {
                _logger.LogInformation("RegisterUser() завершено");
                return new UserModel()
                {
                    Id = newId,
                    Login = user.Login,
                    Password = user.Password,
                    RoleId = user.RoleId
                };
            }
            else
            {
                return null;
            }
        }

        public UserModel GetUser(UserRequest userRequest)
        {
            _logger.LogInformation("GetUser() запуск метода");         
            if (userRequest != null)
            {      
                var user = _userRepository.GetUser(new UserDto()
                {
                    Login = userRequest.Login,
                    Password = userRequest.Password,
                    RoleId = userRequest.RoleId               
                });          

                if (user != null)
                {
                    _logger.LogInformation("GetUser() завершено");
                    return new UserModel()
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Password = user.Password,
                        RoleId = user.RoleId
                    };
                }                
            }
            return null;
        }
        
        public bool SetUser(UserModel user)
        {
            _logger.LogInformation("SetUser() запуск метода");
            return _userRepository.SetUser(new UserDto()
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                RoleId = user.RoleId
            });
        }

        public List<UserModel> GetUserAll()
        {
            _logger.LogInformation("GetUserAll() запуск метода");
            var usersDto = _userRepository.GetUsersAll();
            List<UserModel> users = new List<UserModel>();
            if (usersDto != null)
            {
                foreach (var userDto in usersDto)
                {                    
                    users.Add(new UserModel()
                    {
                        Id = userDto.Id,
                        Login = userDto.Login,
                        Password = userDto.Password,
                        RoleId = userDto.RoleId
                    });
                }
            }
            _logger.LogInformation("GetUserAll() завершено");
            return users;
        }

        public string GenerateJwtToken(string login, string role)
        {
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, login),//"silenceafter"
                    new Claim(JwtRegisteredClaimNames.Email, "silenceafter@mail.com"),
                    new Claim("roles", role)
                };

            byte[] secretBytes = Encoding.UTF8.GetBytes(Constants.SecretKey);
            var key = new SymmetricSecurityKey(secretBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
