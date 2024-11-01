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
        Task<List<BookingResponseDTO>> GetAllBookings();

        Task<BookingResponseDTO> GetBookingByID(int id);

        Task<BookingResponseDTO> CreateNewBooking(CreateNewBookingRequest model);

        Task<BookingResponseDTO> UpdateBooking(int bookingID, UpdateBookingRequest model);

        Task<Booking> DeleteBooking(int id);
       Task<List<BookingResponseDTO>> GetAllBookingInHelper(int helperId);
        Task<List<BookingResponseDTO>> GetAllBookingInUser(int userId);


    }

    public class BookingService : IBookingService
    {
        private readonly UnitOfWork _unitOfWork;
        public BookingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BookingResponseDTO>> GetAllBookings()
        {
            try
            {
                var bookings = _unitOfWork.BookingRepository.GetAllBookingsWithOthers();
                if (bookings.Count() <= 0)
                {
                    return new List<BookingResponseDTO>();
                }

                var response = bookings.Select(b => new BookingResponseDTO
                {
                    Id = b.Id,
                    Price = b.Price,
                    BookingDate = b.BookingDay.ToString("yyyy-mm-dd"),
                    BookingTime = b.BookingDay.ToString("HH:mm:ss"),
                    ServiceDate = b.ServiceDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Address = b.Address,
                    Phone = b.Phone,
                    Note = b.Note,
                    Status = b.Status,
                    Longitude = b.LongItude,
                    Latitude = b.Latitude,
                    HelperName = b.Helper.User.Name,
                    UserName = b.User.Name,
                    ServiceName = b.Service.Name
                });

                return response.ToList();
            }
            catch (Exception ex)
            {
                return new List<BookingResponseDTO>();
            }
        }

        public async Task<BookingResponseDTO> GetBookingByID(int id)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetAllBookingsWithOthers().Where(b => b.Id == id).FirstOrDefaultAsync();
                if (booking == null)
                {
                    return null;
                }

                var response = new BookingResponseDTO
                {
                    Id = booking.Id,
                    Price = booking.Price,
                    BookingDate = booking.BookingDay.ToString("yyyy-mm-dd"),
                    BookingTime = booking.BookingDay.ToString("HH:mm:ss"),
                    ServiceDate = booking.ServiceDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Address = booking.Address,
                    Phone = booking.Phone,
                    Note = booking.Note,
                    Status = booking.Status,
                    Longitude = booking.LongItude,
                    Latitude = booking.Latitude,
                    HelperName = booking.Helper.User.Name,
                    UserName = booking.User.Name,
                    ServiceName = booking.Service.Name
                };

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BookingResponseDTO> CreateNewBooking(CreateNewBookingRequest model)
        {
            try
            {
                var helper = await _unitOfWork.HelperRepository.GetByIdAsync(model.HelperId);
                if (helper == null)
                {
                    return null;
                }

                var user = await _unitOfWork.UserRepository.GetByIdAsync(model.UserId);
                if (user == null)
                {
                    return null;
                }

                var newBookings = new Booking
                {
                    Price = model.Price,
                    BookingDay = DateTime.Now,
                    Address = model.Address,
                    Phone = model.Phone,
                    Note = model.Note,
                    Status = (int)BookingStatusEnums.PENDING,
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
                                               Longitude = booking.LongItude,
                                               Latitude = booking.Latitude,
                                               HelperName = booking.Helper.User.Name,
                                               UserName = booking.User.Name,
                                               ServiceName = booking.Service.Name,
                                               ServiceDate = booking.ServiceDate.ToString("yyyy-MM-dd HH:mm:ss")
                                           });

                    return (BookingResponseDTO)response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BookingResponseDTO> UpdateBooking(int bookingID, UpdateBookingRequest model)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetAllBookingsWithOthers().Where(b => b.Id == bookingID).FirstOrDefaultAsync();
                if (booking == null)
                {
                    return null;
                }

                var helper = await _unitOfWork.HelperRepository.GetByIdAsync(model.HelperId ?? booking.HelperId);
                if (helper == null)
                {
                    return null;
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
                        ServiceDate = booking.ServiceDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        BookingTime = booking.BookingDay.ToString("HH:mm:ss"),
                        Address = booking.Address,
                        Phone = booking.Phone,
                        Note = booking.Note,
                        Status = booking.Status,
                        Longitude = booking.LongItude,
                        Latitude = booking.Latitude,
                        HelperName = booking.Helper.User.Name,
                        UserName = booking.User.Name,
                        ServiceName = booking.Service.Name
                    };

                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Booking> DeleteBooking(int id)
        {
            try
            {
                var bookingExisted = await _unitOfWork.BookingRepository.GetByIdAsync(id);
                if (bookingExisted != null)
                {

                    var result = await _unitOfWork.BookingRepository.RemoveAsync(bookingExisted);
                    if (result)
                    {
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return bookingExisted;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<BookingResponseDTO>> GetAllBookingInUser(int userId)
        {
            try
            {
                var bookings = _unitOfWork.BookingRepository.GetAllBookingsWithOthers();
                var result = bookings.Where(x => x.UserId == userId).ToList();
                if (result.Count() <= 0)
                {
                    return new List<BookingResponseDTO>();
                }

                var response = result.Select(b => new BookingResponseDTO
                {
                    Id = b.Id,
                    Price = b.Price,
                    BookingDate = b.BookingDay.ToString("yyyy-mm-dd"),
                    BookingTime = b.BookingDay.ToString("HH:mm:ss"),
                    ServiceDate = b.ServiceDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Address = b.Address,
                    Phone = b.Phone,
                    Note = b.Note,
                    Status = b.Status,
                    Longitude = b.LongItude,
                    Latitude = b.Latitude,
                    HelperName = b.Helper.User.Name,
                    UserName = b.User.Name,
                    ServiceName = b.Service.Name
                });

                return response.ToList();
            }
            catch (Exception ex)
            {
                return new List<BookingResponseDTO>();
            }

        }



        public async Task<List<BookingResponseDTO>> GetAllBookingInHelper(int helperId)
        {
            try
            {
                var bookings = _unitOfWork.BookingRepository.GetAllBookingsWithOthers();
                var result = bookings.Where(x => x.HelperId == helperId).ToList();
                if (result.Count() <= 0)
                {
                    return new List<BookingResponseDTO>();
                }

                var response = result.Select(b => new BookingResponseDTO
                {
                    Id = b.Id,
                    Price = b.Price,
                    BookingDate = b.BookingDay.ToString("yyyy-mm-dd"),
                    BookingTime = b.BookingDay.ToString("HH:mm:ss"),
                    ServiceDate = b.ServiceDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Address = b.Address,
                    Phone = b.Phone,
                    Note = b.Note,
                    Status = b.Status,
                    Longitude = b.LongItude,
                    Latitude = b.Latitude,
                    HelperName = b.Helper.User.Name,
                    UserName = b.User.Name,
                    ServiceName = b.Service.Name
                });

                return response.ToList();
            }
            catch (Exception ex)
            {
                return new List<BookingResponseDTO>();
            }

        }




    }
}
