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
        Task<CartResponseDTO> AddToCartAsync(int userId, int serviceId, int quantity);
        Task<bool> RemoveCartItemAsync(int cartId);
    }
    public class CartService : ICartService
    {
        private readonly UnitOfWork _unitOfWork;
        public CartService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartResponseDTO> AddToCartAsync(int userId, int serviceId, int quantity)
        {
            // Check if the cart item already exists for the user and service
            var existingCart = await _unitOfWork.CartRepository.GetAllCartWithOthers()
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.ServiceId == serviceId);

            if (existingCart != null)
            {
                // If the item exists, update the quantity
                existingCart.Quantity += quantity;
                _unitOfWork.CartRepository.Update(existingCart);
            }
            else
            {
                // If the item does not exist, create a new cart item
                var newCart = new Cart
                {
                    UserId = userId,
                    ServiceId = serviceId,
                    Quantity = quantity
                };
                await _unitOfWork.CartRepository.CreateAsync(newCart);
            }

            // Retrieve updated cart item for response
            var updatedCart = await _unitOfWork.CartRepository.GetAllCartWithOthers()
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.ServiceId == serviceId);

            return new CartResponseDTO
            {
                CartId = updatedCart.Id,
                ServiceName = updatedCart.Service.Name,
                ServiceImage = updatedCart.Service.Image,
                ServicePrice = updatedCart.Service.Price,
                Quantity = updatedCart.Quantity,
                Subtotal = updatedCart.Service.Price * updatedCart.Quantity
            };
        }


        public async Task<(IEnumerable<CartResponseDTO>, double)> GetCartItemsAsync(int userId)
        {
            var carts = await _unitOfWork.CartRepository.GetAllCartWithOthers()
                .Where(cart => cart.UserId == userId)
                .ToListAsync();

            var cartItems = carts.Select(cart => new CartResponseDTO
            {
                CartId = cart.Id,
                ServiceName = cart.Service.Name,
                ServiceImage = cart.Service.Image,
                ServicePrice = cart.Service.Price,
                Quantity = cart.Quantity,
                Subtotal = cart.Service.Price * cart.Quantity
            });

            var totalSubtotal = cartItems.Sum(item => item.Subtotal);

            return (cartItems, totalSubtotal);
        }

        public async Task<bool> RemoveCartItemAsync(int cartId)
        {
            var cartItem = await _unitOfWork.CartRepository.GetByIdAsync(cartId);
            if (cartItem == null)
            {
                return false;
            }

            _unitOfWork.CartRepository.Remove(cartItem);

            return true;
        }
    }
}
