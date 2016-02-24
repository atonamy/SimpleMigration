/* Copyright 2013 Dario Malfatti https://github.com/darionato/PostgreSqlMigrationSqlGenerator. 
   See LICENSE. */

using System.Text;

namespace EntityFramework.PostgreSql.Interfaces
{
    internal interface IPostgreSqlFragment
    {

        void WriteSql(StringBuilder writer, PostgreSqlGenerator sqlGenerator);

    }
}
