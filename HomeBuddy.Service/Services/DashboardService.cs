using HomeBuddy.Common;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{
    public interface IDashboardService
    {
        Task<DashboardResponse> GetDashboard();
    }
    public class DashboardService : IDashboardService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IFirebaseService _firebaseService;
        public DashboardService(UnitOfWork unitOfWork, IFirebaseService firebaseService)
        {
            _unitOfWork = unitOfWork;
            _firebaseService = firebaseService;

        }

        public async Task<DashboardResponse> GetDashboard()
        {
            var usersCount = await _unitOfWork.UserRepository.CountUsersExceptAdminAsync();
            var bookingsCount = await _unitOfWork.BookingRepository.CountBooking();
            var servicesCount = await _unitOfWork.ServiceRepository.CountService();
            var getTotal = await _unitOfWork.BookingRepository.GetTotal();
            var dashboard = new DashboardResponse
            {
                UserCount = usersCount,
                BookingCount = bookingsCount,
                ServiceCount = servicesCount,
                TotalIncome = getTotal
            };
            return dashboard;
        }
    }
}
