using LingoLibrary.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace LingoLibrary.ApiManagers;

public class OpenAiManager
{
	private readonly string _apiKey;
	private readonly string _baseUrl = "https://api.openai.com/v1/";

	public OpenAiManager(IConfiguration configuration)
    {
		_apiKey = configuration["Api:open_ai_api_key"];
	}

	public async Task<List<WordModel>> GetWords(List<string> lines)
	{
		string context =
			"You will be provided with TV Show script, and your task is to extract a list of smart/ advanced words and phrases from it."
			+ "Verbs write with 'to' at front in bare impersonal form, add articles to countable nouns."
			+ @"List them with their Polish translations in json format eg {""to scribble"": ""bazgrać"",...}.";

		context =
			"Find 10 most advanced english vocabulary in given text. You mustn't list words under C1 english level."
			+ "Verbs write with 'to' at front in bare impersonal form, add articles 'a' or 'an' to countable nouns."
			+ @"List them with their Polish translations in JSON format e.g. {""to scribble"": ""bazgrać"",...}.";


		double maxWords = 2048 * 6.5 / 10 - context.Split().Length;
		maxWords *= 0.7;  // Temp

		var prompts = GetPrompts(lines, maxWords);

		var words = new List<WordModel>();
		//var tasks = new List<Task<List<WordModel>>>();

		//foreach (var prompt in prompts)
		//{
		//	tasks.Add(CallChatGpt(prompt, context));
		//}

		var tasks = prompts.Select(prompt => CallChatGpt(prompt, context));

		Debug.WriteLine("Got Tasks.");

		foreach (var task in tasks)
		{
			words.AddRange(await task);
		}

		return words;
	}

	private IEnumerable<string> GetPrompts(List<string> lines, double maxWords)
	{
		List<string> text = new();
		int counter = 0;

		foreach (string line in lines)
		{
			counter += line.Split().Length;

			if (counter >= maxWords)
			{
				yield return String.Join('.', text);
				counter = 0;
				text = new() { line };
			}
			else
			{
				text.Add(line);
			}
		}

	}

	private async Task<List<WordModel>> CallChatGpt(string text, string context)
	{
		var client = new RestClient($"{_baseUrl}/chat/completions");
		var request = new RestRequest();

		request.AddHeader("Content-Type", "application/json");
		request.AddHeader("Authorization", $"Bearer {_apiKey}");

		var payload = new
		{
			//model = "gpt-3.5-turbo",
			model = "gpt-4",
			messages = new[]
			{
				new { role = "system", content = context },
				new { role = "user", content = text },				
			},
			temperature = 0,
			max_tokens=2048
		};

		var temp = JsonConvert.SerializeObject(payload);
		
		request.AddJsonBody(temp);

		var response = await client.ExecutePostAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception(response.ErrorMessage);
		}

		var json = JsonConvert.DeserializeObject<dynamic>(response.Content);
		string content = json.choices[0].message.content;

		// find only JSON part
		// gpt may occasionaly add some additional info
		// which may cause some errors
		int start = content.IndexOf('{');
		int end = content.LastIndexOf('}');
		content = content.Substring(start, end - start + 1);

		// Chat GPT adds '\' before double quotes
		// e.g. { \"to walk\": \"chodzić\" }
		content = content.Replace("\\", "");

		var words = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

		var output = words.Select(x => new WordModel { English = x.Key, Polish = x.Value }).ToList();

		return output;
	}
}
