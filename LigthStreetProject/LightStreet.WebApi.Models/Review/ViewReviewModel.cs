using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LightStreet.WebApi.Models.Review
{
    public class ViewReviewModel
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public ReviewState State { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string ApplyOn { get; set; }
    }
}
