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
        Task<IBusinessResult> GetById(int id);
        Task<List<HomeBuddy.Data.Models.Service>> GetAll();
        Task<IBusinessResult> Update(HomeBuddy.Data.Models.Service package);
        Task<IBusinessResult> Save(CreateServiceDTO package);

    }

    public class ServiceService : IServiceService
    {
        private readonly UnitOfWork _unitOfWork;
        public ServiceService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            try
            {
                var obj = await _unitOfWork.ServiceRepository.GetByIdAsync(id);

                if (obj == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
                }
                else
                {
                    return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, obj);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXEPTION, ex.Message);
            }
        }

        public async Task<List<HomeBuddy.Data.Models.Service>> GetAll()
        {
            try
            {
                #region Business rule
                #endregion

                var objs = await _unitOfWork.ServiceRepository.GetServicesAsync();

                if (objs == null)
                {
                    return null;
                }
                else
                {
                    return objs;
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
        public async Task<IBusinessResult> Update(HomeBuddy.Data.Models.Service package)
        {
            try
            {
                var packageUpdate = await _unitOfWork.ServiceRepository.UpdateAsync(package);
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
