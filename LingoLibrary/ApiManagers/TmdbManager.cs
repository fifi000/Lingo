using LingoLibrary.Models;
using Newtonsoft.Json;
using RestSharp;
using static System.Net.WebRequestMethods;

namespace LingoLibrary.ApiManagers;

public class TmdbManager
{
	private readonly string _token;
	private readonly string _baseUrl = "https://api.themoviedb.org/3/";
	private readonly string _imageBaseUrl = "https://image.tmdb.org/t/p/w500/";

	public TmdbManager(string token)
    {
		_token = token;
	}

	public async Task<List<SerieModel>> GetSeriesByName(string serieName)
	{
		var options = new RestClientOptions($"{_baseUrl}/search/tv?query={serieName}");
		var client = new RestClient(options);
		var request = new RestRequest();
		
		request.AddHeader("Accept", "application/json");		
		request.AddHeader("Authorization", $"Bearer {_token}");

		var response = await client.GetAsync(request);
		
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception(response.ErrorMessage);
		}

		var json = JsonConvert.DeserializeObject<dynamic>(response.Content);
		
		var output = new List<SerieModel>();

		foreach (var result in json.results)
		{
			var model = new SerieModel();

			try
			{
				model.Id = result.id;
				model.Name = result.name;
				model.Description = result.overview;
				//model.Cover = (result.poster_path is not null) ? $"{_imageBaseUrl}{result.poster_path}" : null;
				model.Cover = $"{_imageBaseUrl}{result?.poster_path}";
				model.ReleaseDate = result.first_air_date;
			}
			catch
			{
				// skip this 
				continue;
			}

			output.Add(model);
		}

		return output;
	}
	
	public async Task<SerieModel> GetSerieById(int id)
	{		
		var options = new RestClientOptions($"{_baseUrl}/tv/{id}");
		var client = new RestClient(options);
		var request = new RestRequest();

		request.AddHeader("Accept", "application/json");
		request.AddHeader("Authorization", $"Bearer {_token}");

		var response = await client.GetAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception(response.ErrorMessage);
		}

		var json = JsonConvert.DeserializeObject<dynamic>(response.Content);

		var output = new SerieModel();

		output.Id = json.id;
		output.Name = json.name;
		output.Description = json.overview;
		output.ReleaseDate = json.first_air_date;
		output.Cover = $"https://image.tmdb.org/t/p/w500/{json.poster_path}";

		output.Seasons = new();

		foreach (var season in json.seasons)
		{
			SeasonModel model = new();

			model.Id = season.id;
			model.Number = season.season_number;
			model.Episodes = await GetEpisodes(output.Id, model.Number);
			output.Seasons.Add(model);
		}

		return output;
	}

	public async Task<List<EpisodeModel>> GetEpisodes(int serieId, int seasonNumber)
	{
		var options = new RestClientOptions($"{_baseUrl}/tv/{serieId}/season/{seasonNumber}");
		var client = new RestClient(options);
		var request = new RestRequest();

		request.AddHeader("Accept", "application/json");
		request.AddHeader("Authorization", $"Bearer {_token}");

		var response = await client.GetAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception(response.ErrorMessage);
		}

		var json = JsonConvert.DeserializeObject<dynamic>(response.Content);

		var output = new List<EpisodeModel>();

		EpisodeModel model;
		foreach (var episode in json.episodes)
		{
			model = new();

			model.Id = episode.id;
			model.Number = episode.episode_number;
			model.Name = episode.name;
			model.Description = episode.overview;
			model.Cover = $"{_imageBaseUrl}{episode.still_path}";

			output.Add(model);
		}

		return output;
	}
}
