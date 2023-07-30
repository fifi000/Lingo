using Microsoft.Extensions.Configuration;
using LingoLibrary.Models;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace Data;

public class LiteDbDataAccess
{
	private readonly string _databasePath;

	public LiteDbDataAccess(IConfiguration configuration)
        {
		_databasePath = configuration.GetConnectionString("LiteDbConnectionString");
	}

	public void CreateSet(SetModel set)
	{
		using var db = new LiteDatabase(_databasePath);

		var sets = db.GetCollection<SetModel>("sets");
		sets.Insert(set);
	}

	public List<SetModel> GetAllSets()
	{
		using var db = new LiteDatabase(_databasePath);

		var sets = db.GetCollection<SetModel>("sets");
		return sets.FindAll().ToList();
	}

	public SetModel GetSet(int episodeId)
	{
		using var db = new LiteDatabase(_databasePath);

		var sets = db.GetCollection<SetModel>("sets");

		// Assuming the combination of SerieId, SeasonId, and EpisodeId uniquely identifies a set
		return sets.FindOne(x => x.EpisodeId == episodeId);
	}

	public bool UpdateSet(SetModel set)
	{
		using var db = new LiteDatabase(_databasePath);

		var sets = db.GetCollection<SetModel>("sets");
		return sets.Update(set);
	}
}
