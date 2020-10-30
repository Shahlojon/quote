using System.ComponentModel.DataAnnotations;

public class QuoteItem
{
    public long Id { get; set; }
    [Required(ErrorMessage = "Write author")]
    public string author { get; set; }
    [Required(ErrorMessage = "Write quote")]
    public string quote { get; set; }
    [Required(ErrorMessage = "Write category")]
    public string category { get; set; }
}