using System.Security.Cryptography;

namespace LingoLibrary;

public static class Helper
{

	/// <summary>
	/// Removes unnecessary lines and splits the subtitles into individual sentences (based on a comma char).
	/// </summary>
	/// <example>
	/// input:
	/// 1
	/// 00:00:00,552 --> 00:00:02,513
	/// Here we go.
	/// Pad thai, no peanuts.
	/// 
	/// 2
	/// 00:00:02,633 --> 00:00:03,969
	/// But does it have peanut oil?
	///
	/// 3
	/// 00:00:04,192 --> 00:00:05,342
	/// I'm not sure.
	/// 
	/// output:
	/// Here we go
	/// Pad thai, no peanuts
	/// But does it have peanut oil? I'm not sure
	/// </example>

	public static List<string> FormatSubtitles(string subtitles)
	{
		var lines = subtitles
			.Split('\n')
			.ToList()
			.Select(x => x.Replace('\n', ' '))
			.Where(ValidLine);

		var text = String.Join(' ', lines);
		return text.Split('.').ToList();
	}

	private static bool ValidLine(string line)
	{
		return (
			String.IsNullOrEmpty(line) == false
			&& Int16.TryParse(line, out _) == false
			&& line.Contains("-->") == false
		);
	}
}
