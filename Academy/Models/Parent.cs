using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Parent
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string? UserName { get; set; }
        public string ParentAddress { get; set; }
        public string? ParentEmail{ get; set; }
        public string ParentPhone { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        //list of children
        public ICollection<Trainee> Children { get; set; } = new List<Trainee>();
      
        public bool IsDeleted { get; set; }

    }
}
