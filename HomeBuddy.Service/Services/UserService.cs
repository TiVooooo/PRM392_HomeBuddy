using HomeBuddy.Common;
using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model;

namespace HomeBuddy.Service.Services
{

    public interface IUserService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> Create(UserDTO request);
        Task<IBusinessResult> Update(int userId, UserDTO request);
        Task<IBusinessResult> Delete(int userId);
    }

    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, users);
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
                return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);

            return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, user);
        }

        public async Task<IBusinessResult> Create(UserDTO request)
        {
            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                Phone = request.Phone,
                Gender = request.Gender,
                Address = request.Address,
                Avatar = request.Avatar,
                Role = request.Role,
            };
            await _unitOfWork.UserRepository.CreateAsync(user);

            return new BusinessResult(Const.SUCCESS_CREATE, Const.SUCCESS_CREATE_MSG);
        }

        public async Task<IBusinessResult> Update(int userId, UserDTO request)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
            }

            user.Email = request.Email;
            user.Password = request.Password;
            user.Name = request.Name;
            user.Phone = request.Phone;
            user.Gender = request.Gender;
            user.Address = request.Address;
            user.Avatar = request.Avatar;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            return new BusinessResult(Const.SUCCESS_UDATE, Const.SUCCESS_UDATE_MSG, user);
        }

        public async Task<IBusinessResult> Delete(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
            }

            await _unitOfWork.UserRepository.RemoveAsync(user);
            return new BusinessResult(Const.SUCCESS_DELETE, Const.SUCCESS_DELETE_MSG);
        }
    }
}
