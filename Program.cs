using System;
using System.Data.SqlClient;

namespace SQLConsole
{
    delegate void RegularQueries();
    delegate void ParameterQueries(string artist, string title);

    class Program
    {
        static void Main(string[] args)
        {
            SQLFunctions funcs = new SQLFunctions();
            RegularQueries getAll = new RegularQueries(funcs.GetAllQuery);
            RegularQueries deleteAll = new RegularQueries(funcs.DeleteAllQuery);
            ParameterQueries insert = new ParameterQueries(funcs.InsertIntoQuery);
            ParameterQueries delete = new ParameterQueries(funcs.SelectWithParamQuery);
            ParameterQueries edit = new ParameterQueries(funcs.EditWithParamQuery);

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
                    Console.WriteLine("Edit \t-\t Edit a record");
                    Console.WriteLine("Exit\n");
                    string ans = Console.ReadLine();

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
                            string artistA = Console.ReadLine();
                            Console.WriteLine("Enter title");
                            string titleA = Console.ReadLine();
                            insert(artistA, titleA);
                            break;
                        case "Delete":
                            Console.WriteLine("Enter artist to remove");
                            string artistB = Console.ReadLine();
                            Console.WriteLine("Enter title to remove");
                            string titleB = Console.ReadLine();
                            delete(artistB, titleB);
                            break;
                        case "Edit":
                            Console.WriteLine("Enter artist to edit");
                            string artistC = Console.ReadLine();
                            Console.WriteLine("Enter new title");
                            string titleC = Console.ReadLine();
                            edit(artistC, titleC);
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
                    string continueAnswer = Console.ReadLine();

                    if (continueAnswer == "No")
                    {
                        flag = true;
                        Console.WriteLine("Ending SQL CLI...");
                    }
                    else if (continueAnswer == "Yes")
                    {
                        flag = false;
                    }

                    Console.ReadLine();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("Error: {0}", e.InnerException);
            }
        }
    }

    interface ISQLFunctions
    {
        void GetAllQuery();
        void DeleteAllQuery();
        void InsertIntoQuery(string artist, string title);
        void DeleteWithParamQuery(string artist, string title);
        void SelectWithParamQuery(string artist, string title);
        void EditWithParamQuery(string artist, string title);
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
            string connectionString = "Server=localhost;Database=StoreMusic;Integrated Security=true;";
            string query = "SELECT * FROM MusicTable";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);

                    int result = command.ExecuteNonQuery();

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
            string connectionString = "Server=localhost;Database=StoreMusic;Integrated Security=true;";
            string query = "DELETE FROM MusicTable";

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
            string connectionString = "Server=localhost;Database=StoreMusic;Integrated Security=true;";
            string query = "INSERT INTO MusicTable(Artist, Title) VALUES(@artist, @title)";

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
            string connectionString = "Server=localhost;Database=StoreMusic;Integrated Security=true;";
            string query = "DELETE FROM MusicTable WHERE Title = @title AND Artist = @artist";

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
            string connectionString = "Server=localhost;Database=StoreMusic;Integrated Security=true;";
            string query = "Select * FROM MusicTable WHERE Artist = @artist AND TItle = @title";

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

        public void EditWithParamQuery(string artist, string title)
        {
            string connectionString = "Server=localhost;Database=StoreMusic;Integrated Security=true;";
            string query = "UPDATE MusicTable SET Title = @title WHERE Artist = @artist";

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

        ~SQLFunctions()
        {
            // Destructor
        }
    }
}
