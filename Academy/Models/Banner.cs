﻿
namespace Academy.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
