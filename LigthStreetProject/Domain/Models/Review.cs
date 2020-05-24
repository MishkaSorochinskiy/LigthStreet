using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public ReviewState State { get; set; }
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public int PointId { get; set; }
        public virtual Point Point { get; set; }
        public int ApplyOnId { get; set; }
        public virtual User ApplyOn { get; set; }
    }
}
