﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Abstracciones
{
    public sealed record Error(string code, string? description = null)
    {
        public static readonly Error None = new(string.Empty);
    }
}
