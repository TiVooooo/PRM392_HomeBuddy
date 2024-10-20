﻿using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{
    public interface IHelperService
    {
        public Task<IBusinessResult> GetAll();
        public Task<IBusinessResult> GetById(int id);
    }
    public class HelperService : IHelperService
    {
        private readonly UnitOfWork _unitOfWork;

        public HelperService(UnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork; 
        }

        public async Task<IBusinessResult> GetAll()
        {
            var helpers = await _unitOfWork.HelperRepository.GetAllAsync();

            return new BusinessResult(200, "Get all helpers!", helpers);
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            var helper = await _unitOfWork.HelperRepository.GetByIdAsync(id);

            return new BusinessResult(200, "Get helper!", helper);
        }
    }
}
