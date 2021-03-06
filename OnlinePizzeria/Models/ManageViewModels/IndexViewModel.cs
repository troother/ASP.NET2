﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Zip code can only contain numbers")]
        [Display(Name = "ZipCode")]
        public int ZipCode { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        public string StatusMessage { get; set; }
    }
}
