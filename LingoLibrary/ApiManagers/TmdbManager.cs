using LingoLibrary.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace LingoLibrary.ApiManagers;

public class TmdbManager
{
	private readonly string _token;
	private readonly string _baseUrl = "https://api.themoviedb.org/3/";

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
			output.Add(new SerieModel
			{
				Id = result.id,
				Name = result.name,
				Description = result.overview,
				Cover = $"https://image.tmdb.org/t/p/w500/{result.poster_path}",
				//Cover = "https://image.tmdb.org/t/p/w500/ooBGRQBdbGzBxAVfExiO8r7kloA.jpg",
				ReleaseDate = result.first_air_date
			});
		}


		return output;
	}
	
	public async Task<SerieModel> GetSerieById(int id)
	{
		throw new NotImplementedException();
	}
}
