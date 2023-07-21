using LingoLibrary.Models;
using RestSharp;
using System.Collections.Generic;

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
		serieName = serieName.Replace(" ", "%20");

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

		var results = new List<SerieModel>();

		return results;
	}
}
