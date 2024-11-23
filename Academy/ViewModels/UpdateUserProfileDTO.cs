namespace Academy.ViewModels
{
    public class UpdateUserProfileDTO
    {
        public string UserId { get; set; } = null!;
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public IFormFile? Image { get; set; }
    }
}
