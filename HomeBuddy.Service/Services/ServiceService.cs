using Firebase.Auth;
using HomeBuddy.Common;
using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{
    public interface IServiceService
    {
        Task<ServiceModel> GetById(int id);
        Task<List<ServiceModel>> GetAll();
        Task<IBusinessResult> Update(int id, CreateServiceDTO package);
        Task<IBusinessResult> Save(CreateServiceDTO package);

    }

    public class ServiceService : IServiceService
    {
        private readonly UnitOfWork _unitOfWork;
        public ServiceService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceModel> GetById(int id)
        {
            try
            {
                var obj = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
                var helper = _unitOfWork.HelperRepository.GetById(obj.HelperId);
                var user = _unitOfWork.UserRepository.FindByCondition(u => u.Id == helper.UserId).FirstOrDefault();

                var service = new ServiceModel
                {
                    Id = id,
                    Description = obj.Description,
                    Duration = obj.Duration,
                    HelperName = user.Name,
                    Name = obj.Name,
                    Image = obj.Image,
                    Price = obj.Price
                };

                if (service == null)
                {
                 return   null;
                }
                else
                {
                    return service;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ServiceModel>> GetAll()
        {
            try
            {
                var objs = await _unitOfWork.ServiceRepository.GetServicesAsync();
                var list = new List<ServiceModel>();
                foreach (var obj in objs) {
                    var helper = _unitOfWork.HelperRepository.GetById(obj.HelperId);
                    var user = _unitOfWork.UserRepository.FindByCondition(u=> u.Id ==  helper.UserId).FirstOrDefault();
                    var service = new ServiceModel
                    {
                        Id = obj.Id,
                        Description = obj.Description,
                        Duration = obj.Duration,
                        HelperName = user.Name,
                        Name = obj.Name,
                        Image = obj.Image,
                        Price = obj.Price
                    };
                    list.Add(service);
                }

                if (list == null)
                {
                    return null;
                }
                else
                {
                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IBusinessResult> Save(CreateServiceDTO package)
        {
            try
            {

                var rs = new HomeBuddy.Data.Models.Service
                {
                    Name = package.Name,
                    Description = package.Description,
                    Duration = package.Duration,
                    HelperId = package.HelperId,
                    Price = package.Price
                };

                var newPackage = await _unitOfWork.ServiceRepository.CreateAsync(rs);
                if (newPackage > 0)
                {
                    return new BusinessResult(Const.SUCCESS_CREATE, Const.SUCCESS_CREATE_MSG, newPackage);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                {
                    return new BusinessResult(Const.ERROR_EXEPTION, ex.Message);
                }
            }
        }
        public async Task<IBusinessResult> Update(int id, CreateServiceDTO package)
        {
            try
            {
                var service = _unitOfWork.ServiceRepository.GetById(id);
                service.Description = package.Description;
                service.Duration = package.Duration;
                service.HelperId = package.HelperId;
                service.Price = package.Price;
                service.Name = package.Name;
                var packageUpdate = await _unitOfWork.ServiceRepository.UpdateAsync(service);
                if (packageUpdate > 0)
                {
                    return new BusinessResult(Const.SUCCESS_UDATE, Const.SUCCESS_UDATE_MSG, packageUpdate);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UDATE, Const.FAIL_UDATE_MSG);
                }
            }
            catch (Exception ex)
            {

                return new BusinessResult(Const.ERROR_EXEPTION, ex.Message);
            }
        }

    }
}
