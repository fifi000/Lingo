using LingoLibrary.Models;
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
			&& (line.Contains("<font") || line.Contains("font>")) == false
		);
	}

	public static List<SetModel> GetFakeSetModels()
	{
		return new()
		{
			new ()
			{
				SerieId = 1,
				SeasonId = 1,
				EpisodeId = 1,
				Words = new ()
				{
					new WordModel { English = "Hello", Polish = "Cześć" },
					new WordModel { English = "World", Polish = "Świat" },
					new WordModel { English = "Goodbye", Polish = "Do widzenia" }
				}
			},
			new ()
			{
				SerieId = 1,
				SeasonId = 1,
				EpisodeId = 2,
				Words = new List<WordModel>
				{
					new WordModel { English = "House", Polish = "Dom" },
					new WordModel { English = "Car", Polish = "Samochód" },
					new WordModel { English = "Tree", Polish = "Drzewo" }
				}
			},
			new ()
			{
				SerieId = 2,
				SeasonId = 1,
				EpisodeId = 1,
				Words = new List<WordModel>
				{
					new WordModel { English = "Cat", Polish = "Kot" },
					new WordModel { English = "Dog", Polish = "Pies" },
					new WordModel { English = "Bird", Polish = "Ptak" }
				}
			}
		};
	}
}
