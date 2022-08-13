using System;
using System.Collections.Generic;

namespace haber1.Model
{
    public partial class NewsCategory
    {
        public string CategoryDescription { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
