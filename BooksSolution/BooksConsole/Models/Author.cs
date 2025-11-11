using System.ComponentModel.DataAnnotations;

namespace BooksConsole.Models;

public class Author
{
    public int AuthorId { get; set; }

    [Required]
    public string AuthorName { get; set; } = default!;

    public ICollection<Title> Titles { get; set; } = new List<Title>();
}
