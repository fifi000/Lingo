namespace LingoApp.Models;

public class SeriesModel
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
    public List<SeasonModel> Seasons { get; set; }
}
