﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request.Pet
{
    public class CreatePetRequest
    {
		public string? Name { get; set; }
		public string? Species { get; set; }
		public int CustomerId { get; set; }
		public DateTime? Dob { get; set; }
		public bool? Gender { get; set; }
		public string? Generic { get; set; }
		public string? Description { get; set; }
    }
}
