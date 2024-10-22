﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBuddy.Service.Model
{
    public class FireBaseConfigurationModel
    {
        public string? ApiKey { get; set; }
        public string? AuthEmail { get; set; }
        public string? AuthPassword { get; set; }
        public string? StorageBucket { get; set; }
        public string? AuthDomain { get; set; }
        public string? ProjectId { get; set; }
        public string? MessagingSenderId { get; set; }
        public string? AppId { get; set; }
        public string? MeasurementId { get; set; }
    }
}
