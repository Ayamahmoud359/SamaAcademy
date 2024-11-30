namespace Academy.ViewModels
{
    public class EditSubscriptionVM
    {
        public int SubscriptionId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool? IsActive { get; set; }
        

    }
}
