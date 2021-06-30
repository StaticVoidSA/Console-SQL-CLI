using System;
using System.Data.SqlClient;

namespace SQLConsole
{
    delegate void RegularQueries();
    delegate void ParameterQueries(string artist, string title);
    delegate void EditQuery(string artist, string title, string _artist, string _title);

    class Program
    {
        public static string connectionString = "Server=localhost;Database=StoreMusic;Integrated Security=true;";
        public static string query = "SELECT * FROM MusicTable";
        
        static void Main(string[] args)
        {
            SQLFunctions funcs = new SQLFunctions();
            RegularQueries getAll = new RegularQueries(funcs.GetAllQuery);
            RegularQueries deleteAll = new RegularQueries(funcs.DeleteAllQuery);
            ParameterQueries insert = new ParameterQueries(funcs.InsertIntoQuery);
            ParameterQueries delete = new ParameterQueries(funcs.DeleteWithParamQuery);
            ParameterQueries select = new ParameterQueries(funcs.SelectWithParamQuery);
            EditQuery edit = new EditQuery(funcs.EditWithParamQuery);

            bool flag = false;

            Console.WriteLine("Welcome to my SQL CLI");
            
            try
            {
                while (flag == false)
                {
                    Console.WriteLine("What would you like to do?\n");
                    Console.WriteLine("All \t-\t Get all records");
                    Console.WriteLine("DAll \t-\t Delete all records");
                    Console.WriteLine("Insert \t-\t Insert new record");
                    Console.WriteLine("Delete \t-\t Delete a record");
                    Console.WriteLine("Select \t-\t Select a record");
                    Console.WriteLine("Edit \t-\t Edit a record");
                    Console.WriteLine("Exit\n");
                    string ans = Console.ReadLine().Trim();
                    string artist = null, title = null;
                    string _artist = null, _title = null;

                    switch (ans)
                    {
                        case "All":
                            Console.WriteLine("Getting all records...");
                            getAll();
                            break;
                        case "DAll":
                            Console.WriteLine("Deleting all records");
                            deleteAll();
                            break;
                        case "Insert":
                            Console.WriteLine("Enter artist");
                            artist = Console.ReadLine().Trim();
                            Console.WriteLine("Enter title");
                            title = Console.ReadLine().Trim();
                            insert(artist, title);
                            break;
                        case "Delete":
                            Console.WriteLine("Enter artist to remove");
                            artist = Console.ReadLine().Trim();
                            Console.WriteLine("Enter title to remove");
                            title = Console.ReadLine().Trim();
                            delete(artist, title);
                            break;
                        case "Select":
                            Console.WriteLine("Enter artist");
                            artist = Console.ReadLine().Trim();
                            Console.WriteLine("Enter title");
                            title = Console.ReadLine().Trim();
                            select(artist, title);
                            break;
                        case "Edit":
                            Console.WriteLine("Enter artist to edit");
                            artist = Console.ReadLine().Trim();
                            Console.WriteLine("Enter song title");
                            title = Console.ReadLine().Trim();
                            Console.WriteLine("Enter new artist name");
                            _artist = Console.ReadLine().Trim();
                            Console.WriteLine("Enter new title");
                            _title = Console.ReadLine().Trim();
                            edit(artist, title, _artist, _title);
                            break;
                        case "Exit":
                            Console.WriteLine("Exiting...");
                            flag = true;
                            break;
                        default:
                            Console.WriteLine("Please select a valid value");
                            break;
                    }


                    Console.WriteLine("Do you wish to try again? Yes? No?");
                    string continueAnswer = Console.ReadLine().Trim();

                    if (continueAnswer == "No")
                    {
                        flag = true;
                        Console.WriteLine("Ending SQL CLI...");
                    }
                    else if (continueAnswer == "Yes")
                    {
                        flag = false;
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }

            Console.ReadLine();
        }
    }

    interface ISQLFunctions
    {
        void GetAllQuery();
        void DeleteAllQuery();
        void InsertIntoQuery(string artist, string title);
        void DeleteWithParamQuery(string artist, string title);
        void SelectWithParamQuery(string artist, string title);
        void EditWithParamQuery(string artist, string title, string _artist, string _title);
    }

    class SQLFunctions : ISQLFunctions
    {
        public SQLFunctions()
        {
            // Default constructor
        }

        static SQLFunctions()
        {
            // Static constructor
        }

        public void GetAllQuery()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("There are no records in datatable");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            string artist = reader["Artist"].ToString();
                            string title = reader["Title"].ToString();

                            Console.WriteLine("Artist: {0}\nTitle: {1}", artist, title);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        public void DeleteAllQuery()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    int result = command.ExecuteNonQuery();

                    if (result == 0)
                    {
                        Console.WriteLine("No matching records in datatable");
                    }
                    else
                    {
                        Console.WriteLine("All records removed from datatable. {0} rows affected", result);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }
        }

        public void InsertIntoQuery(string artist, string title)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@artist", artist);
                    command.Parameters.AddWithValue("@title", title);
                    
                    int result = command.ExecuteNonQuery();
                    Console.WriteLine("{0} rows affected", result);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }
        }

        public void DeleteWithParamQuery(string artist, string title)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@artist", artist);
                    command.Parameters.AddWithValue("@title", title);

                    int result = command.ExecuteNonQuery();
                    Console.WriteLine("{0} rows affected", result);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }
        }

        public void SelectWithParamQuery(string artist, string title)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@artist", artist);
                    command.Parameters.AddWithValue("@title", title);

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("There are no records in datatable");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            string Artist = reader["Artist"].ToString();
                            string Title = reader["Title"].ToString();

                            Console.WriteLine("Artist: {0}\nTitle: {1}", Artist, Title);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }
        }

        public void EditWithParamQuery(string artist, string title, string _artist, string _title)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@artist", artist);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@_artist", _artist);
                    command.Parameters.AddWithValue("@_title", _title);

                    int result = command.ExecuteNonQuery();

                    Console.WriteLine("{0} rows affected", result);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }
        }

        ~SQLFunctions()
        {
            // Destructor
        }
    }
}
