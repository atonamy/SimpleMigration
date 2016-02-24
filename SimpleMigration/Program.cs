using System; 
using System.Linq;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Infrastructure;
using System.Resources;
using System.IO;
using System.Text.RegularExpressions;

namespace SimpleMigration
{
	class MainClass
	{
		const string MIGRATION_NAME = "AllSimpleMigration";

		public static void Main (string[] args)
		{
			// USER INPUT /////////////////////////////////////////////////////////////////////////////////

			// Always first create a new database migration with DatabaseStep.ADD_MIGRATION,
			// and include the created files in the project and set resource file to EmbeddedResource. 
			// After creating a migration run UPDATE_DATABASE to update the database.

			const MigrationStep step = MigrationStep.CREATE_SCRIPT; /* Never forget: MIGRATION_NAME ! */

			// Specify the name of the database migration in case of ADD-MIGRATION.
			// Note: Make sure to create a new name for each new migration.
			//       After creating migration include the files in the folder by right clicking on 
			//       Oogstplanner.Migrations and selecting "Add files from folder". Then add the .cs, .resx and
			//       .Designer.cs files with the name specified below.
			//       Last but not least set the .resx file's build action to EmbeddedResource by right
			//       clicking on it.
			// Make sure that the setup-database.sh script has run to create the database and user.


			// END USER INPUT /////////////////////////////////////////////////////////////////////////////


			// Get executing path from which the location of the Update_Scripts and new Migrations can be determined.
			var executingPath = AppDomain.CurrentDomain.BaseDirectory; 

			// Add a new migration (PowerShell: Add-Migration)
			if (step == MigrationStep.ADD_MIGRATION) {

				// Initialize the wrapper classes around the Entity Framework PowerShell API.
				var config = new Configuration();
				var scaffolder = new MigrationScaffolder(config); 
				var migration = scaffolder.Scaffold(MIGRATION_NAME);

				// Place migration code in main project "Migrations" folder and migration scripts in "App_Data"
				var migrationsPath = Regex.Replace(executingPath, "bin/.*", "");

				// Write migrations
				File.WriteAllText(migrationsPath + MIGRATION_NAME + ".cs", migration.UserCode);
				File.WriteAllText(migrationsPath + MIGRATION_NAME + ".Designer.cs", migration.DesignerCode);

				using (var writer = new ResXResourceWriter (migrationsPath + MIGRATION_NAME + ".resx")) 
				{
					foreach (var resource in migration.Resources) 
					{
						writer.AddResource(resource.Key, resource.Value);
					}
				}
				Console.WriteLine("EF code migration {0} written to Migrations folder...\n\n" +
					"Next step is to include the .cs, .resx and .Designer.cs file in the project" + 
					"by right clicking on the project and selecting " +  
					"\"Add files from folder.\"\n" +
					"Then right click on {1}.resx and set build action to \"EmbeddedResource\""
					, migration.MigrationId, MIGRATION_NAME);
			}

			else if (step == MigrationStep.CREATE_SCRIPT)
			{

				var config = new Configuration();
				var migrator = new DbMigrator(config);
				var scriptor = new MigratorScriptingDecorator(migrator);

				// Determine name of the previous run migration if exists.
				string lastMigration = migrator.GetDatabaseMigrations().FirstOrDefault();

				// Get the script 
				string script = scriptor.ScriptUpdate(sourceMigration: lastMigration, targetMigration: MIGRATION_NAME);

				// Create the PostgreSQL update script based on last migration on database and 
				// current migration.
				string formattedScript = string.Format
					("/* * * * * * * * * * * * * * * * * * * * * * *\n" +
						" *\n" +
						" * Migration:\t\t{0}\n *\n" +
						" * Date and time:\t{1}\n" +
						" *\n" +
						" * * * * * * * * * * * * * * * * * * * * * * */\n\n" +
						"{2}", 
						MIGRATION_NAME, 
						DateTime.Now,
						script);

				// Write string to file in Migrations folder of main project
				var migrationsPath = Regex.Replace(executingPath, "bin/.*", "");
				File.WriteAllText(migrationsPath + MIGRATION_NAME + ".sql", formattedScript);
				Console.WriteLine("Script {0}.sql successfully generated.", 
					MIGRATION_NAME);
			}

			// If a new migration is created the database can be updated. (PowerShell: Update-Database)
			else if (step == MigrationStep.UPDATE_DATABASE)
			{
				var config = new Configuration();
				var migrator = new DbMigrator(config);

				// Write to database
				migrator.Update();

				// Show which migrations are applied.
				var migrationNames = string.Join(", ", migrator.GetDatabaseMigrations().ToArray());
				Console.WriteLine("Applied migration {0} to database.", migrationNames);
			}
		}

		enum MigrationStep 
		{
			ADD_MIGRATION,
			CREATE_SCRIPT,
			UPDATE_DATABASE
		}
	}
}
