﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Events
{
    public class PaymentSucceededEvent
    {
        public int OrderId { get; set; }
    }
}
