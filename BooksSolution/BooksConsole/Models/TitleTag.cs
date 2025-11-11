namespace BooksConsole.Models;

public class TitleTag
{
    public int TitleTagId { get; set; }

    public int TitleId { get; set; }
    public Title Title { get; set; } = default!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}
