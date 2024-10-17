﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HomeBuddy.Data.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public bool Gender { get; set; }

    public string Address { get; set; }

    public string Avatar { get; set; }

    public string Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Chat> ChatReceivers { get; set; } = new List<Chat>();

    public virtual ICollection<Chat> ChatSenders { get; set; } = new List<Chat>();

    public virtual ICollection<Helper> Helpers { get; set; } = new List<Helper>();

    public virtual ICollection<User> InverseParent { get; set; } = new List<User>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual User Parent { get; set; }
}