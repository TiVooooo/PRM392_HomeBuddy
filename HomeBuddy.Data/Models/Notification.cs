﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HomeBuddy.Data.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string Tittle { get; set; }

    public string Description { get; set; }

    public bool Status { get; set; }

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; }
}