namespace LingoLibrary.Models;

public class SerieModel
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
    public List<SeasonModel> Seasons { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string Cover { get; set; }
}
