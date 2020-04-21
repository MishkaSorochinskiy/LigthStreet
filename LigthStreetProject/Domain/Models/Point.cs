using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Point: BaseModel
    {
        public Point(double latitude,
            double longtitude)
        {
            Latitude = latitude;
            Longtitude = longtitude;
        }
        [Required]
        public double Latitude { get; set; }

        [Required]  
        public double Longtitude { get; set; }
    }
}
