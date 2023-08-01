using LingoLibrary.Models;
using LiteDB;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Data;

public class SetData
{
	private readonly LiteDbDataAccess _dataAccess;

	public SetData(LiteDbDataAccess dataAccess)
    {
		_dataAccess = dataAccess;
	}

    public void CreateSet(SetModel set)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>("sets");
		sets.Insert(set);
	}

	public List<SetModel> GetAllSets()
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>("sets");
		return sets.FindAll().ToList();
	}

	public SetModel GetSet(int episodeId)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>("sets");

		return sets.FindOne(x => x.EpisodeId == episodeId);
	}

	public bool UpdateSet(SetModel set)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>("sets");
		return sets.Update(set);
	}

	public bool DeleteSet(int episodeId)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>("sets");

		var set = sets.FindOne(x => x.EpisodeId == episodeId);

		if (set is null)
		{
			Debug.WriteLine($"Could not find set for episode id {episodeId}");
			return false;
		}

		sets.Delete(set.Id);
		return true;
	}

}
