﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HomeBuddy.Data.Models;

public partial class Message
{
    public int Id { get; set; }

    public string MessageText { get; set; }

    public DateTime SentTime { get; set; }

    public int ChatId { get; set; }

    public int SenderId { get; set; }

    public virtual Chat Chat { get; set; }

    public virtual User Sender { get; set; }
}