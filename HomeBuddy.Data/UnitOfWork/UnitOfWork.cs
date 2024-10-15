using HomeBuddy.Data.Models;
using HomeBuddy.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Data.UnitOfWork
{
    public class UnitOfWork
    {
        public PRM392_HomeBuddyContext _unitOfWorkContext;
        private BookingRepository _bookingRepository;
        private CartRepository _cartRepository;
        private ChatRepository _chatRepository;
        private HelperRepository _helperRepository;
        private MessageRepository _messageRepository;
        private NotificationRepository _notificationRepository;
        private ServiceRepository _serviceRepository;
        private UserRepository _userRepository;
        public UnitOfWork(PRM392_HomeBuddyContext unitOfWorkContext)
        {
            _unitOfWorkContext = unitOfWorkContext;
        }

        public BookingRepository BookingRepository
        {
            get
            {
                return _bookingRepository ??= new BookingRepository(_unitOfWorkContext);
            }
        }

        public CartRepository CartRepository
        {
            get
            {
                return _cartRepository ??= new CartRepository(_unitOfWorkContext);
            }
        }

        public ChatRepository ChatRepository
        {
            get
            {
                return _chatRepository ??= new ChatRepository(_unitOfWorkContext);
            }
        }

        public HelperRepository HelperRepository
        {
            get
            {
                return _helperRepository ??= new HelperRepository(_unitOfWorkContext);
            }
        }

        public MessageRepository MessageRepository
        {
            get
            {
                return _messageRepository ??= new MessageRepository(_unitOfWorkContext);
            }
        }

        public NotificationRepository NotificationRepository
        {
            get
            {
                return _notificationRepository ??= new NotificationRepository(_unitOfWorkContext);
            }
        }

        public ServiceRepository ServiceRepository
        {
            get
            {
                return _serviceRepository ??= new ServiceRepository(_unitOfWorkContext);
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_unitOfWorkContext);
            }
        }
    }
}
