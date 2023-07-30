using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace LingoLibrary.ApiManagers;

public class OpenSubtitlesManager
{
	private readonly string _apiKey;
	private readonly string _username;
	private readonly string _password;
	private readonly string _baseUrl = "https://api.opensubtitles.com/api/v1/";

	private string _token;

	public OpenSubtitlesManager(IConfiguration configuration)
    {
		_apiKey = configuration["Api:open_subtitles:api_key"];
		_username = configuration["Api:open_subtitles:username"];
		_password = configuration["Api:open_subtitles:password"];
	}

	public async Task<string> GetSubtitles(int episodeId)
	{
		int fileId = await GetSubtitlesFileId(episodeId);

		await LogIn();
		
		string link = await GetDownloadLink(fileId);

		return await DownloadSubtitles(link);
	}

	private async Task LogIn()
	{
		var client = new RestClient($"{_baseUrl}/login");
		var request = new RestRequest();

		request.AddHeader("Content-Type", "application/json");
		request.AddHeader("Accept", "application/json");
		request.AddHeader("Api-Key", $"{_apiKey}");

		var payload = new
		{
			username = _username,
			password = _password
		};

		request.AddParameter("application/json", payload, ParameterType.RequestBody);
		
		var response = await client.ExecuteAsync(request, Method.Post);
		
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception(response.ErrorMessage);
		}

		var json = JsonConvert.DeserializeObject<dynamic>(response.Content);

		_token = (string)json.token;

	}

	private async Task<string> GetDownloadLink(int fileId)
	{
		var client = new RestClient($"{_baseUrl}/download");
		var request = new RestRequest();

		request.AddHeader("Content-Type", "application/json");
		request.AddHeader("Accept", "application/json");
		request.AddHeader("Api-Key", $"{_apiKey}");
		request.AddHeader("Authorization", $"Bearer {_token}");

		var payload = new { file_id = fileId };

		request.AddParameter("application/json", payload, ParameterType.RequestBody);

		var response = await client.ExecuteAsync(request, Method.Post);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception(response.ErrorMessage);
		}

		var json = JsonConvert.DeserializeObject<dynamic>(response.Content);

		return (string)json.link;
	}

	private async Task<int> GetSubtitlesFileId(int episodeId)
	{
		var options = new RestClientOptions($"{_baseUrl}/subtitles?tmdb_id={episodeId}");
		var client = new RestClient(options);
		var request = new RestRequest();

		request.AddHeader("Accept", "application/json");
		request.AddHeader("Api-Key", $"{_apiKey}");
		request.AddHeader("Content-Type", "application/json");

		var response = await client.GetAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception(response.ErrorMessage);
		}

		var json = JsonConvert.DeserializeObject<dynamic>(response.Content);

		return (int)json.data[0].attributes.files[0].file_id;
	}

	private async Task<string> DownloadSubtitles(string link)
	{
		var client = new RestClient(link);
		var request = new RestRequest();

		var response = await client.ExecuteAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception(response.ErrorMessage);
		}

		return response.Content;
	}
}
