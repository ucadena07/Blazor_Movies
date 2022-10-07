﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.Dtos
{
    public class PaginatedResponse<T>
    {
        public T Response { get; set; }
        public int TotalAmountPages { get; set; }
    }
}
