using AutoMapper;
using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using HomeBuddy.Common;
using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model;
using Microsoft.AspNetCore.Http;

namespace HomeBuddy.Service.Services
{

    public interface IUserService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> Create(CreateUserDTO request);
        Task<IBusinessResult> Update(int userId, UserDTO request);
        Task<IBusinessResult> Delete(int userId);
        Task<IBusinessResult> EditAvatar(int id, IFormFile avatar);
        Task<IBusinessResult> CreateHelper(CreateHelperDTO request);
    }

    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IFirebaseService _firebaseService;
        public UserService(UnitOfWork unitOfWork, IFirebaseService firebaseService)
        {
            _unitOfWork = unitOfWork;            _firebaseService = firebaseService;

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

        public async Task<IBusinessResult> Create(CreateUserDTO request)
        {
            
            if (CheckEmailExist(request.Email))
            {
                return new BusinessResult("Email đã tồn tại.");
            }
            if (CheckPhoneExist(request.Phone))
            {
                return new BusinessResult("Số điện thoại đã tồn tại.");
            }
            var user = new Data.Models.User
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                Phone = request.Phone,
                Gender = request.Gender,
                Address = request.Address,
                CreatedAt = DateTime.Now,
                Avatar = "https://inkythuatso.com/uploads/thumbnails/800/2023/03/9-anh-dai-dien-trang-inkythuatso-03-15-27-03.jpg",
                Role = "User"
            };
            await _unitOfWork.UserRepository.CreateAsync(user);

            return new BusinessResult(Const.SUCCESS_CREATE, Const.SUCCESS_CREATE_MSG);
        }
        public async Task<IBusinessResult> CreateHelper(CreateHelperDTO request)
        {

                if (CheckEmailExist(request.Email))
                {
                    return new BusinessResult("Email đã tồn tại.");
                }
                if (CheckPhoneExist(request.Phone))
                {
                    return new BusinessResult("Số điện thoại đã tồn tại.");
                }
                var user = new Data.Models.User
                {
                    Email = request.Email,
                    Password = request.Password,
                    Name = request.Name,
                    Phone = request.Phone,
                    Gender = request.Gender,
                    Address = request.Address,
                    CreatedAt = DateTime.Now,
                    Avatar = "https://inkythuatso.com/uploads/thumbnails/800/2023/03/9-anh-dai-dien-trang-inkythuatso-03-15-27-03.jpg",
                    Role = "Helper"
                };
                await _unitOfWork.UserRepository.CreateAsync(user);
                var helper = new Helper
                {
                    Status = true,
                    Skill = request.Skill,
                    UserId = user.Id
                };
                await _unitOfWork.HelperRepository.CreateAsync(helper);
                return new BusinessResult(Const.SUCCESS_CREATE, Const.SUCCESS_CREATE_MSG);
        }

        public async Task<IBusinessResult> EditAvatar(int id, IFormFile avatar)
        {
            var checkedUser = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (checkedUser == null) return new BusinessResult(Const.FAIL_READ, Const.FAIL_READ_MSG);

            var avatarUrl = await _firebaseService.UploadImageToFirebaseAsync(avatar, "avatars");
            checkedUser.Avatar = avatarUrl;

            await _unitOfWork.UserRepository.UpdateAsync(checkedUser);
            return new BusinessResult(Const.SUCCESS_UDATE, "Avatar updated successfully");
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

        private bool CheckEmailExist(string email)
        {
            var result = _unitOfWork.UserRepository.GetAll().Any(x => x.Email.ToLower().Trim() == email.ToLower().Trim());
            return result;
        }

        private bool CheckPhoneExist(string phone)
        {
            var result = _unitOfWork.UserRepository.GetAll().Any(x => x.Phone.Trim() == phone.Trim());
            return result;
        }
    }
}
