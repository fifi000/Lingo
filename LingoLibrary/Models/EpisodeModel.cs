namespace LingoLibrary.Models;

public class EpisodeModel
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string Title { get; set; }
	public List<WordModel> Words { get; set; }
}
