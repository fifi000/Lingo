using Microsoft.Extensions.Configuration;
using LingoLibrary.Models;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Data;

public class LiteDbDataAccess
{
	private readonly string _databasePath;

	public LiteDbDataAccess(IConfiguration configuration)
    {
		_databasePath = configuration.GetConnectionString("LiteDbConnectionString");
	}

	public LiteDatabase GetDatabase()
	{
		return new LiteDatabase(_databasePath);
	}
}
