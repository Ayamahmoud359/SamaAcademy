using Academy.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.ViewModels
{
    public class AttendanceDTO
    {
        public int AbsenceId { get; set; }
        public bool IsAbsent { get; set; }
        public DateTime? AbsenceDate { get; set; }
        public string? Type { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }
        public bool IsDeleted { get; set; }
        public int? TrainerId { get; set; }
    }
}
