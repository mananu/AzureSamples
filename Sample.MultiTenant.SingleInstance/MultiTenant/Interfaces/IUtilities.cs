﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MultiTenant.Interfaces
{
    public interface IUtilities
    {
        void ResetEventDates(string connString);
    }
}
