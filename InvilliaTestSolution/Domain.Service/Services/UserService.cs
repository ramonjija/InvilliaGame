using Domain.Interfaces;
using Domain.Model.Entity;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class UserService : IUserService
    {
        private IUnityOfWork _unitOfWork;
        private IPasswordService _passwordService;
        public UserService(IUnityOfWork unitOfWork, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }
        public async Task<IServiceResult<User>> CreateUser(string name, string password, int userTypeId)
        {
            try
            {
                var serviceResult = new ServiceResult<User>();
                var type = await _unitOfWork.UserTypes.GetById(userTypeId);
                if (type == null)
                {
                    serviceResult.AddMessage($"User Type Not Found. 'UserTypeId: {userTypeId}'");
                }

                var existingUser = await _unitOfWork.Users.GetUserByName(name);
                if (existingUser != null)
                {
                    serviceResult.AddMessage($"There's already a user with this name. 'Name: {name}'");
                }

                if (!serviceResult.Success)
                    return serviceResult;

                var user = new User(name, type, _passwordService.HashPassword(password));
                if (type.TypeId == (int)UserTypeEnum.Friend)
                    user = new Friend(user);
                var newUser = await _unitOfWork.Users.Create(user);
                await _unitOfWork.Commit();
                serviceResult.SetResult(newUser);

                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IServiceResult<User>> DeleteUser(int userId)
        {
            try
            {
                var serviceResult = new ServiceResult<User>();
                var deletedUser = await _unitOfWork.Users.Delete(userId);
                if (deletedUser == null)
                {
                    serviceResult.AddMessage("User Not Found");
                    return serviceResult;
                }
                await _unitOfWork.Commit();
                serviceResult.SetResult(deletedUser);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IServiceResult<IEnumerable<User>>> GetUser()
        {
            try
            {
                var serviceResult = new ServiceResult<IEnumerable<User>>();
                var users = await _unitOfWork.Users.GetUsersWithType();
                serviceResult.SetResult(users);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IServiceResult<User>> GetUser(int userId)
        {
            try
            {
                var serviceResult = new ServiceResult<User>();
                var user = await _unitOfWork.Users.GetUserWithType(userId);
                if (user == null)
                {
                    serviceResult.AddMessage("User Not Found");
                    return serviceResult;
                }
                serviceResult.SetResult(user);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IServiceResult<User>> GetUser(string userName, string password)
        {
            try
            {
                var serviceResult = new ServiceResult<User>();
                var user = await _unitOfWork.Users.GetUserWithType(userName, password);
                if (user == null)
                {
                    serviceResult.AddMessage("User Not Found");
                    return serviceResult;
                }
                if (!_passwordService.IsPasswordValid(password, user.PasswordHash))
                {
                    serviceResult.AddMessage("Incorrect Password");
                    return serviceResult;
                }

                serviceResult.SetResult(user);
                return serviceResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IServiceResult<User>> UpdateUser(int userId, string userName, string password, int userTypeId)
        {
            try
            {
                var serviceResult = new ServiceResult<User>();
                var user = await _unitOfWork.Users.GetById(userId);
                if (user == null)
                    serviceResult.AddMessage($"User Not Found '{userName}'");

                var userType = await _unitOfWork.UserTypes.GetById(userTypeId);
                if (userType == null)
                    serviceResult.AddMessage($"User Type Not Found. 'UserTypeId: {userTypeId}'");

                if (!serviceResult.Success)
                    return serviceResult;

                user.Update(userName, userType, _passwordService.HashPassword(password));

                var updatedUser = _unitOfWork.Users.Update(user);
                await _unitOfWork.Commit();

                serviceResult.SetResult(updatedUser);
                return serviceResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
