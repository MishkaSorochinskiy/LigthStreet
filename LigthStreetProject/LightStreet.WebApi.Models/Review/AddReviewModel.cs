using System;
using System.Collections.Generic;
using System.Text;

namespace LightStreet.WebApi.Models.Review
{
    public class AddReviewModel
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public int ApplyOnId { get; set; }
        public int PointId { get; set; }
    }
}
