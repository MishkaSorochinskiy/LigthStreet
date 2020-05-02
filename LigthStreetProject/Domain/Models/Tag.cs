using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string Name { get; set; }
    }
}
