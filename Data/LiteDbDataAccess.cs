using Microsoft.Extensions.Configuration;
using LiteDB;
using System;
using System.IO;

namespace Data;

public class LiteDbDataAccess
{
	private readonly string _databasePath;

	public LiteDbDataAccess(IConfiguration configuration)
    {
		var dataDir = (string)AppDomain.CurrentDomain.GetData("DataDirectory");
		var db = configuration.GetConnectionString("LiteDbConnectionString");
		
		_databasePath = Path.Combine(dataDir, db);
	}

	public LiteDatabase GetDatabase()
	{
		return new LiteDatabase(_databasePath);
	}
}
