using Microsoft.Data.Sqlite;

namespace PrivateWiki.Data
{
    class PageAccess : IPageAccess
    {
        const string DATABASE_NAME = "pages.db";
        const string TABLE_NAME = "Pages";

        const string ID = "id";
        const string PAGE = "Page";
        private const string DATE_OF_CREATION = "CreationDate";
        private const string DATE_OF_LAST_CHANGE = "ChangeDate";

        public void InitDatabase()
        {
            using (var db = new SqliteConnection($"Filename={DATABASE_NAME}"))
            {
                db.Open();

                string tableCommand = $"CREATE TABLE IF NOT EXISTS {TABLE_NAME}({ID} TEXT PRIMARY KEY, {PAGE} TEXT NULL)";

                var createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public void AddPage(string markdown)
        {
            using (var db = new SqliteConnection($"Filename={DATABASE_NAME}"))
            {
                db.Open();

                string tableCommand = $"INSERT INTO {TABLE_NAME} VALUES (1, @Page)";

                var addPage = new SqliteCommand()
                {
                    CommandText = tableCommand,
                    Connection = db,
                };
                addPage.Parameters.AddWithValue("@Page", markdown);

                try
                {
                    addPage.ExecuteReader();
                }
                catch (SqliteException e)
                {
                    return;
                }
                db.Close();
                
            }
        }

        public void UpdatePage(string id, string markdown)
        {
            using (var db = new SqliteConnection($"Filename={DATABASE_NAME}"))
            {
                db.Open();

                var tableCommand = $"UPDATE {TABLE_NAME} SET {ID} = {id}, {PAGE} {markdown} WHERE {ID} = {id}";

                var updatePageCommand = new SqliteCommand()
                {
                    CommandText = tableCommand,
                    Connection = db
                };

                try
                {
                    updatePageCommand.ExecuteReader();
                }
                catch (SqliteException e)
                {
                    return;
                }
                db.Close();
            }
        }

        public string GetPage(string id)
        {
            string markdown = null;

            using (var db = new SqliteConnection($"Filename={DATABASE_NAME}"))
            {
                db.Open();

                var tableCommand = new SqliteCommand($"SELECT {PAGE} FROM {TABLE_NAME} WHERE {ID}={id}", db);
                SqliteDataReader query;

                try
                {
                    query = tableCommand.ExecuteReader();
                }
                catch (SqliteException e)
                {
                    return "Error";
                }
                while(query.Read())
                {
                    markdown = query.GetString(0);
                }
                db.Close();
            }

            return markdown;
        }

        public void DropTable()
        {
            using (var db = new SqliteConnection($"Filename={DATABASE_NAME}"))
            {
                db.Open();

                string tableCommand = $"DROP TABLE IF EXISTS {TABLE_NAME}";

                var dropTable = new SqliteCommand(tableCommand, db);

                dropTable.ExecuteReader();
            }
        }
    }
}
