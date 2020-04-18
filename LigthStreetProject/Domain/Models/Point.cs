using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Point: BaseModel
    {
        public Point(double latitude,
            double altitude)
        {
            Latitude = latitude;
            Altitude = altitude;
        }
        [Required]
        public double Latitude { get; set; }

        [Required]  
        public double Altitude { get; set; }
    }
}
