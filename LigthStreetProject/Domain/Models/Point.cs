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
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public string Street { get; set; }
        public List<Review> Reviews { get; set; }

    }
}
