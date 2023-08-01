using LingoLibrary.Models;
using LiteDB;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Data;

public class SetData
{
	private readonly LiteDbDataAccess _dataAccess;
	private const string CollectionName = "sets";
	public SetData(LiteDbDataAccess dataAccess)
    {
		_dataAccess = dataAccess;
	}

    public void CreateSet(SetModel set)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>(CollectionName);
		sets.Insert(set);
	}

	public List<SetModel> GetAllSets()
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>(CollectionName);
		return sets.FindAll().ToList();
	}

	public SetModel GetSet(int episodeId)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>(CollectionName);

		return sets.FindOne(x => x.EpisodeId == episodeId);
	}

	public bool UpdateSet(SetModel set)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>(CollectionName);
		return sets.Update(set);
	}

	public bool DeleteSet(int episodeId)
	{
		using var db = _dataAccess.GetDatabase();

		var sets = db.GetCollection<SetModel>(CollectionName);

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
