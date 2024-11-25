namespace Academy.ViewModels
{
    public class AddUserVM
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string EntityType { get; set; }
        public string Password { get; set; }
        public int? BranchId { get; set; }
    }
}
