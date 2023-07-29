namespace LingoLibrary.Models;

public class SetModel
{
    public int SerieId { get; set; }
    public int SeasonId { get; set; }
    public int EpisodeId { get; set; }
    public List<WordModel> Words { get; set; }
}
