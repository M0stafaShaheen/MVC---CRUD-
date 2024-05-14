using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book2.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Display(Name = "Book Title") ]
        public string ?Title { get; set; }

        public string? Description { get; set; }

        [ForeignKey("Auther")]
        public int? AutherId { get; set; }

        public string? PublishDate { get; set; }    

        public bool ? IsDeleted { get; set; }   

        public virtual Auther? Auther { get; set; } 



    }
}

