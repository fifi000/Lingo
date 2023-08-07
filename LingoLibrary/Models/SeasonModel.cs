namespace LingoLibrary.Models;

public class SeasonModel
{
	public int Id { get; set; }
	public int Number { get; set; }
	public List<EpisodeModel> Episodes { get; set; }
}
