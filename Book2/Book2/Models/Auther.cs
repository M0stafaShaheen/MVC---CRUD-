namespace Book2.Models
{
    public class Auther
    {

        public int ?Id { get; set; } 
        public string? Name { get; set; }
        
        public virtual List<Book>?Books { get; set; } 

    }
}
