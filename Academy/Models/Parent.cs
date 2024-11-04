using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Parent
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string ParentAddress { get; set; }
        public string ParentEmail{ get; set; }
        public string ParentPhone { get; set; }
        public string? Image { get; set; }
        public bool? IsActive { get; set; }
        //list of children
        public ICollection<Child> Children { get; set; } = new List<Child>();
        ///Branch Id
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

    }
}
