using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class MovieQueryObject
    {
        public string? Title { get; set; } = null;
        public string? GenreName { get; set; } = null;
        public string? SortBy { get; set; } = "Title";
        public bool IsDescending { get; set; } = false;
    }
}