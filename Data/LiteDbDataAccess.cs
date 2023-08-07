using Microsoft.Extensions.Configuration;
using LiteDB;
using System;

namespace Data;

public class LiteDbDataAccess
{
	private readonly string _databasePath;

	public LiteDbDataAccess(IConfiguration configuration)
    {
		_databasePath = (string)AppDomain.CurrentDomain.GetData("DataDirectory")
			+ configuration.GetConnectionString("LiteDbConnectionString");
	}

	public LiteDatabase GetDatabase()
	{
		return new LiteDatabase(_databasePath);
	}
}
