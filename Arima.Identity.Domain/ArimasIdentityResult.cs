﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arima.Identity.Domain
{
    public class ArimasIdentityResult : IdentityResult
    {
        public ArimasIdentityResult(bool sucesso)
        {
            Succeeded = sucesso;
        }
    }
}
