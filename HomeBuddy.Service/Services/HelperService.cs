using HomeBuddy.Common;
using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{
    public interface IHelperService
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> ChangeStatus(int helperId);
        Task<IBusinessResult> GetHelperAddressThroughCart(int cartID);
        
    }
    public class HelperService : IHelperService
    {
        private readonly UnitOfWork _unitOfWork;

        public HelperService(UnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork; 
        }
        
        public async Task<IBusinessResult> ChangeStatus(int helperId)
        {
            var helper = await _unitOfWork.HelperRepository.GetByIdAsync(helperId);

            if (helper == null)
            {
                return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
            }

            if(helper.Status == true)
            {
                helper.Status = false;
            }
            else
            {
                helper.Status = true;
            }

            await _unitOfWork.HelperRepository.UpdateAsync(helper);

            return new BusinessResult(Const.SUCCESS_UDATE, $"Status updated successfully", helper);
        }
        public async Task<IBusinessResult> GetAll()
        {
            var helpers = await _unitOfWork.HelperRepository.GetHelpersWithParentAsync();

            return new BusinessResult(200, "Get all helpers!", helpers);
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            var helper = await _unitOfWork.HelperRepository.GetByIdAsync(id);

            return new BusinessResult(200, "Get helper!", helper);
        }

        public async Task<IBusinessResult> GetHelperAddressThroughCart(int cartID)
        {
            var helper = _unitOfWork.CartRepository.GetAllCartWithOthers();

            var helperInCart = helper.FirstOrDefault(c => c.Id == cartID);

            if (helperInCart != null && helperInCart.Service != null && helperInCart.Service.Helper != null && helperInCart.Service.Helper.User != null)
            {
                return new BusinessResult(200, "Get helper address!", new
                {
                    address = helperInCart.Service.Helper.User.Address
                });
            }

            return null;
        }
    }
}
