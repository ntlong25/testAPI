using WebBlog.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebBlog.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using WebBlog.Business.Authorization;
using WebBlog.Business.Helpers;
using WebBlog.Data.Models.Responses;
using WebBlog.Data.Models.Requests;

namespace WebBlog.Business.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        private readonly ILogger<UserService> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public UserService(IUnitOfWork unitOfWork,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings,
            ILogger<UserService> logger,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _unitOfWork = unitOfWork;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public UserResponse GetCurrentUser()
        {
            return new UserResponse(_httpContextAccessor.HttpContext.Items["User"] as User);
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = _unitOfWork.userRepo.List(u => u.email == model.email && u.isDelete == false).SingleOrDefault();

            // validate
            if (user == null || !GetMD5(model.password).Equals(user.passwordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);
            user.lastLogin = DateTime.Now;

            // remove old refresh tokens from user
            removeOldRefreshTokens(user);

            // save changes to db
            _unitOfWork.userRepo.Update(user);
            _unitOfWork.Complete();

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                _unitOfWork.userRepo.Update(user);
                _unitOfWork.Complete();
            }

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from user
            removeOldRefreshTokens(user);

            // save changes to db
            _unitOfWork.userRepo.Update(user);
            _unitOfWork.Complete();

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public void RevokeToken(string token, string ipAddress)
        {
            var user = getUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new AppException("Invalid token");

            // revoke token and save
            revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            _unitOfWork.userRepo.Update(user);
            _unitOfWork.Complete();
        }

        private User getUserByRefreshToken(string token)
        {
            var user = _unitOfWork.userRepo.List(u => u.RefreshTokens.Any(t => t.Token == token)).SingleOrDefault();

            if (user == null)
                throw new AppException("Invalid token");

            return user;
        }

        private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void removeOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    revokeRefreshToken(childToken, ipAddress, reason);
                else
                    revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        public UserResponse CreateUser(User user)
        {
            try
            {
                int userExist = _unitOfWork.userRepo.List(u => u.email.Equals(user.email)).Count();
                if (userExist > 0) throw new Exception("User was existed");
                var result = new User
                {
                    firstName = user.firstName,
                    middleName = user.middleName,
                    lastName = user.lastName,
                    phoneNumber = user.phoneNumber,
                    email = user.email,
                    passwordHash = GetMD5(user.passwordHash),
                    registeredAt = DateTime.Now,
                    lastLogin = DateTime.Now,
                    intro = user.intro,
                    profile = user.profile,
                    isDelete = false,
                    roleId = user.roleId,
                };
                _unitOfWork.userRepo.Create(result);
                _unitOfWork.Complete();
                return new UserResponse(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public UserResponse UpdateUser(User user)
        {
            try
            {
                var result = _unitOfWork.userRepo.Get(user.ID);
                if (result != null)
                {
                    result.firstName = user.firstName;
                    result.middleName = user.middleName;
                    result.lastName = user.lastName;
                    result.phoneNumber = user.phoneNumber;
                    result.email = user.email;
                    result.passwordHash = GetMD5(user.passwordHash);
                    result.registeredAt = user.registeredAt;
                    result.lastLogin = user.lastLogin;
                    result.intro = user.intro;
                    result.profile = user.profile;
                    result.isDelete = false;
                    result.roleId = user.roleId;

                    _unitOfWork.userRepo.Update(result);
                    _unitOfWork.Complete();
                }
                else
                {
                    throw new Exception($"Id {user.ID} not found.");
                }
                return new UserResponse(user);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public IEnumerable<User> GetUser()
        {
            return _unitOfWork.userRepo.GetAll();
        }
        public User GetUser(long id)
        {
            try
            {
                _logger.LogInformation("get by id");
                var result = _unitOfWork.userRepo.Get(id);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteUser(long id)
        {
            try
            {
                _unitOfWork.userRepo.Delete(id);
                _unitOfWork.Complete();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public User Login(string email, string passwordHash)
        {
            try
            {
                User user = _unitOfWork.userRepo.Login(email, passwordHash);
                _unitOfWork.Complete();
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
