using System.ComponentModel.DataAnnotations;

namespace BooksConsole.Models;

public class Tag
{
    public int TagId { get; set; }

    [Required]
    public string TagName { get; set; } = default!;

    public ICollection<TitleTag> TitleTags { get; set; } = new List<TitleTag>();
}
