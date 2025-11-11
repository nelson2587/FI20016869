using System.ComponentModel.DataAnnotations;

namespace BooksConsole.Models;

public class Title
{
    public int TitleId { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = default!;

    [Required]
    public string TitleName { get; set; } = default!;

    public ICollection<TitleTag> TitleTags { get; set; } = new List<TitleTag>();
}
