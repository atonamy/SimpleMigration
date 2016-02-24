using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Infrastructure;
using SimpleData;

namespace SimpleMigration
{
	public class Configuration :  DbMigrationsConfiguration<SimpleDataContext>
	{
		public Configuration ()
		{
			TargetDatabase = new DbConnectionInfo("NpgsqlContext");
			AutomaticMigrationsEnabled = false;
			SetSqlGenerator("Npgsql", new PostgreSqlMigrationSqlGenerator());
		}
	}
}

