namespace Academy.ViewModels
{
    public class UpdateChildProfileDTO
    {
        public int TraineeId { get; set; } 
        public string? TraineeName { get; set; }
        public string? TraineePhone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? TraineeAddress { get; set; }
    }
}
