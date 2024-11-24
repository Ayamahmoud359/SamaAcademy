using System.ComponentModel.DataAnnotations;

namespace Academy.DTO
{
    public class BannerVM
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Title is Required")]

        [Display(Name = "Title")]
        public string? Title { get; set; }
       
        public bool IsActive { get; set; }
       
      
    }
}
