namespace LingoLibrary;

public static class Helper
{

	/// <summary>
	/// Removes unnecessary lines and splits the subtitles into individual sentences (based on a comma char).
	/// </summary>
	public static List<string> FormatSubtitles(string subtitles)
	{
		var lines = subtitles
			.Split('\n')
			.ToList()
			.Select(x => x.Replace('\n', ' '))
			.Where(IsValidLine);

		var text = String.Join(' ', lines);
		return text.Split('.').Where(x => !String.IsNullOrEmpty(x)).ToList();
	}

	private static bool IsValidLine(string line)
	{
		return (
			String.IsNullOrEmpty(line) == false
			&& Int16.TryParse(line, out _) == false
			&& line.Contains("-->") == false
			&& (line.Contains("<font") || line.Contains("font>")) == false
		);
	}
}
