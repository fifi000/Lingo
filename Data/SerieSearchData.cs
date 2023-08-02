using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data;

public class SerieSearchData
{
	private readonly LiteDbDataAccess _dataAccess;
	private const string CollectionName = "searched_series";

	public SerieSearchData(LiteDbDataAccess dataAccess)
	{
		_dataAccess = dataAccess;
	}

	public List<int> GetAllSearchedSeries(int top)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SerieSearchModel>(CollectionName);

		return sets
			.FindAll()
			.OrderByDescending(x => x.LastSearch)
			.Take(top)
			.Select(x => x.Id)
			.ToList();
	}

	public void UpsertSerie(int serieId, DateTime searchDate)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SerieSearchModel>(CollectionName);

		sets.Upsert(new SerieSearchModel { Id = serieId, LastSearch = searchDate });
	}

	public void UpsertSerie(int serieId)
	{
		UpsertSerie(serieId, DateTime.UtcNow);
	}

	public void DeleteSerie(int serieId)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SerieSearchModel>(CollectionName);

		sets.Delete(serieId);
	}

}
