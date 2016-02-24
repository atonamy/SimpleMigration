using System;
using System.Data.Entity;
using SimpleData.Models;
using System.Configuration;

namespace SimpleData
{
	public class SimpleDataContext : DbContext
	{
		public SimpleDataContext () : base(GetConnectionString("NpgsqlContext"))
		{
		}
			
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// PostgreSQL uses schema public by default.
			modelBuilder.HasDefaultSchema("public");
		}

		public DbSet<SimpleTable> SimpleData { get; set; }


		public static string GetConnectionString(string connection_string_name) 
		{
			string connString = 
				ConfigurationManager.ConnectionStrings[connection_string_name].ConnectionString;
			return connString;
		}
	}
}

