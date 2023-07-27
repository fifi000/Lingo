namespace LingoLibrary.Models;

public class EpisodeModel
{
	public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
	public string Description { get; set; }
	public string Cover { get; set; }
	public List<WordModel> Words { get; set; }
}
