using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloRepeat.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required (ErrorMessage="Bosh saxlamaq olmaz"), StringLength(50, ErrorMessage = "Maksimum 50 simvol istifade oluna biler")]
        public string Name { get; set; }
        public string Description { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
        public ICollection<Product> Products { get; set; } 
    }
}
