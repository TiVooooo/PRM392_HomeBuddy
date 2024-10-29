using HomeBuddy.Data.Models;
using HomeBuddy.Data.UnitOfWork;
using HomeBuddy.Service.Model.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Services
{
    public interface ICartService
    {
        Task<(IEnumerable<CartResponseDTO>, double)> GetCartItemsAsync(int userId);
    }
    public class CartService : ICartService
    {
        private readonly UnitOfWork _unitOfWork;
        public CartService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(IEnumerable<CartResponseDTO>, double)> GetCartItemsAsync(int userId)
        {
            var carts = await _unitOfWork.CartRepository.GetAllCartWithOthers()
                .Where(cart => cart.UserId == userId)
                .ToListAsync();

            var cartItems = carts.Select(cart => new CartResponseDTO
            {
                ServiceName = cart.Service.Name,
                ServiceImage = cart.Service.Image,
                ServicePrice = cart.Service.Price,
                Quantity = cart.Quantity,
                Subtotal = cart.Service.Price * cart.Quantity
            });

            var totalSubtotal = cartItems.Sum(item => item.Subtotal);

            return (cartItems, totalSubtotal);
        }
    }
}
