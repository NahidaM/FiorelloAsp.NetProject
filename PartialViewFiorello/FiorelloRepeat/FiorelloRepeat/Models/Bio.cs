﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloRepeat.Models
{
    public class Bio
    {
        public int Id { get; set; }
        [Required]
        public string Logo { get; set; } 
        public string Facebook { get; set; } 
        public string Instagram { get; set; } 
        public string Pinterest { get; set; } 
        public string Tumblr { get; set; } 
        public string LinkedIn { get; set; } 
       
    }
}
