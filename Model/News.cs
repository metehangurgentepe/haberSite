using System;
using System.Collections.Generic;

namespace haber1.Model
{
    public partial class News
    {
        public int CategoryId { get; set; }
        public DateTime DatePosted { get; set; }
        public string NewsContent { get; set; } = null!;
        public int NewsStatus { get; set; }
        public string? NewsTitle { get; set; }
        public int NewsId { get; set; }
        public int UserId { get; set; }
        public string? Pictures { get; set; }
    }
}
