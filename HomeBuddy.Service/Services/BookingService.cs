using HomeBuddy.Common;
using HomeBuddy.Common.Enums;
using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Base;
using HomeBuddy.Service.Model.RequestDTO;
using HomeBuddy.Service.Model.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{
    public interface IBookingService
    {
        Task<IBusinessResult> GetAllBookings();

        Task<IBusinessResult> GetBookingByID(int id);

        Task<IBusinessResult> CreateNewBooking(CreateNewBookingRequest model);

        Task<IBusinessResult> UpdateBooking(int bookingID, UpdateBookingRequest model);

        Task<IBusinessResult> DeleteBooking(int id);
    }

    public class BookingService : IBookingService
    {
        private readonly UnitOfWork _unitOfWork;
        public BookingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBusinessResult> GetAllBookings()
        {
            try
            {
                var bookings = _unitOfWork.BookingRepository.GetAllBookingsWithOthers();
                if (bookings.Count() <= 0)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
                }

                var response = bookings.Select(b => new BookingResponseDTO
                {
                    Id = b.Id,
                    Price = b.Price,
                    BookingDate = b.BookingDay.ToString("yyyy-mm-dd"),
                    BookingTime = b.BookingDay.ToString("HH:mm:ss"),
                    ServiceDate = b.ServiceDate.ToString("yyyy-mm-dd"),
                    Address = b.Address,
                    Phone = b.Phone,
                    Note = b.Note,
                    Status = b.Status,
                    LongItude = b.LongItude,
                    Latitude = b.Latitude,
                    HelperName = b.Helper.User.Name,
                    UserName = b.User.Name,
                });

                return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, response);
            } 
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetBookingByID(int id)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetAllBookingsWithOthers().Where(b => b.Id == id).FirstOrDefaultAsync();
                if(booking == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
                }

                var response = new BookingResponseDTO
                {
                    Id = booking.Id,
                    Price = booking.Price,
                    BookingDate = booking.BookingDay.ToString("yyyy-mm-dd"),
                    BookingTime = booking.BookingDay.ToString("HH:mm:ss"),
                    ServiceDate = booking.ServiceDate.ToString("HH:mm:ss"),
                    Address = booking.Address,
                    Phone = booking.Phone,
                    Note = booking.Note,
                    Status = booking.Status,
                    LongItude = booking.LongItude,
                    Latitude = booking.Latitude,
                    HelperName = booking.Helper.User.Name,
                    UserName = booking.User.Name,
                };

                return new BusinessResult(Const.SUCCESS_READ, Const.SUCCESS_READ_MSG, response);
            }
            catch(Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> CreateNewBooking(CreateNewBookingRequest model)
        {
            try
            {
                var helper = await _unitOfWork.HelperRepository.GetByIdAsync(model.HelperId);
                if (helper == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, "Helper does not existed !");
                }

                var cart = await _unitOfWork.CartRepository.GetByIdAsync(model.CartId);
                if (cart == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, "Cart does not existed !");
                }

                var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, "User does not existed !");
                }

                var newBookings = new Booking
                {
                    Price = model.Price,
                    BookingDay = DateTime.Now,
                    Address = model.Address,
                    Phone = model.Phone,
                    Note = model.Note,
                    Status = (int) BookingStatusEnums.PENDING,
                    ServiceDate = model.ServiceDate,
                    HelperId = model.HelperId,
                    UserId = model.UserId,
                };

                var result = await _unitOfWork.BookingRepository.CreateAsync(newBookings);
                if (result > 0)
                {
                    var bookings = _unitOfWork.BookingRepository.GetAllBookingsWithOthers();
                    var response = bookings.Where(b => b.Id == newBookings.Id)
                                           .Select(booking => new BookingResponseDTO
                    {
                        Id = booking.Id,
                        Price = booking.Price,
                        BookingDate = booking.BookingDay.ToString("yyyy-mm-dd"),
                        BookingTime = booking.BookingDay.ToString("HH:mm:ss"),
                        Address = booking.Address,
                        Phone = booking.Phone,
                        Note = booking.Note,
                        Status = booking.Status,
                        LongItude = booking.LongItude,
                        Latitude = booking.Latitude,
                        HelperName = booking.Helper.User.Name,
                        UserName = booking.User.Name,
                    });

                    return new BusinessResult(Const.SUCCESS_CREATE, Const.SUCCESS_CREATE_MSG, response);
                } 
                else
                {
                    return new BusinessResult(Const.FAIL_CREATE, Const.FAIL_CREATE_MSG);
                }
            } 
            catch(Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateBooking(int bookingID, UpdateBookingRequest model)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetAllBookingsWithOthers().Where(b => b.Id == bookingID).FirstOrDefaultAsync();
                if (booking == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, "Booking does not existed !");
                }

                var helper = await _unitOfWork.HelperRepository.GetByIdAsync(model.HelperId ?? booking.HelperId);
                if (helper == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, "Helper does not existed !");
                }

                var cart = await _unitOfWork.CartRepository.GetByIdAsync(model.CartId ?? booking.CartId);
                if (cart == null)
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, "Cart does not existed !");
                }

                booking.Price = model.Price ?? booking.Price;
                booking.Address = model.Address ?? booking.Address;
                booking.Phone = model.Phone ?? booking.Phone;
                booking.Note = model.Note ?? booking.Note;
                booking.LongItude = model.LongItude ?? booking.LongItude;
                booking.Latitude = model.Latitude ?? booking.Latitude;
                booking.HelperId = model.HelperId ?? booking.HelperId;

                var result = await _unitOfWork.BookingRepository.UpdateAsync(booking);
                if (result > 0)
                {
                    var response = new BookingResponseDTO
                    {
                        Id = booking.Id,
                        Price = booking.Price,
                        BookingDate = booking.BookingDay.ToString("yyyy-mm-dd"),
                        BookingTime = booking.BookingDay.ToString("HH:mm:ss"),
                        Address = booking.Address,
                        Phone = booking.Phone,
                        Note = booking.Note,
                        Status = booking.Status,
                        LongItude = booking.LongItude,
                        Latitude = booking.Latitude,
                        HelperName = booking.Helper.User.Name,
                        UserName = booking.User.Name,
                    };

                    return new BusinessResult(Const.SUCCESS_UDATE, Const.SUCCESS_UDATE_MSG, response);
                }
                else
                {
                    return new BusinessResult(Const.FAIL_UDATE, Const.FAIL_UDATE_MSG);
                }
            }
            catch(Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXEPTION, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteBooking(int id)
        {
            try
            {
                var bookingExisted = await _unitOfWork.BookingRepository.GetByIdAsync(id);
                if (bookingExisted != null)
                {

                    var result = await _unitOfWork.BookingRepository.RemoveAsync(bookingExisted);
                    if (result)
                    {
                        return new BusinessResult(Const.SUCCESS_DELETE, Const.SUCCESS_DELETE_MSG);
                    }
                    else
                    {
                        return new BusinessResult(Const.FAIL_DELETE, Const.FAIL_DELETE_MSG);
                    }
                }
                else
                {
                    return new BusinessResult(Const.WARNING_NO_DATA, Const.WARNING_NO_DATA_MSG);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(Const.ERROR_EXEPTION, ex.Message);
            }
        }

    }
}
