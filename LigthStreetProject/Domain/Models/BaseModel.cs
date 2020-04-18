using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class BaseModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
