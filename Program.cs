using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQLCLI
{
    delegate void RegularQueries();
    delegate void ParameterQueries(string artist, string title);
    delegate void EditQuery(string artist, string title, string _artist, string _title);
    delegate void PopulateDT();

    class Program
    {
        static void Main(string[] args)
        {
            SQLFunctions funcs = new SQLFunctions();
            RegularQueries getAll = new RegularQueries(funcs.GetAllQuery);
            RegularQueries deleteAll = new RegularQueries(funcs.DeleteAllQuery);
            ParameterQueries insert = new ParameterQueries(funcs.InsertIntoQuery);
            ParameterQueries select = new ParameterQueries(funcs.SelectByParamQuery);
            ParameterQueries delete = new ParameterQueries(funcs.DeleteByParamQuery);
            PopulateDT populate = new PopulateDT(funcs.PopulateDT);
            EditQuery edit = new EditQuery(funcs.EditByParamQuery);

            Console.WriteLine("Welcome to my SQL CLI\n");
            Console.WriteLine("Please enter your name\n");
            string name = Console.ReadLine();
            DateTime myDate = DateTime.Now;
            Console.WriteLine($"\nHello {name}");
            Console.WriteLine("Date: {0:d}\nTime: {0:t}", myDate);

            bool flag = false;

            try
            {
                while (flag == false)
                {
                    Console.WriteLine("What would you like to do?\n");
                    Console.WriteLine("Get \t-\tGet all records");
                    Console.WriteLine("DAll \t-\tDelete all records");
                    Console.WriteLine("Insert \t-\tInsert a new record");
                    Console.WriteLine("Select \t-\tSelect record");
                    Console.WriteLine("Delete \t-\tDelete a record");
                    Console.WriteLine("Edit \t-\tEdit a record");
                    Console.WriteLine("Pop \t-\tPopulate datatable with existing data");
                    Console.WriteLine("Exit \t-\tEnd application\n");
                    string ans = Console.ReadLine().Trim();

                    switch (ans)
                    {
                        case "Get":
                            getAll();
                            break;
                        case "DAll":
                            deleteAll();
                            break;
                        case "Insert":
                            Console.WriteLine("Enter artist");
                            string artistA = Console.ReadLine();
                            Console.WriteLine("Enter title");
                            string titleA = Console.ReadLine();
                            insert(artistA, titleA);
                            break;
                        case "Select":
                            Console.WriteLine("Enter artist");
                            string artistB = Console.ReadLine();
                            Console.WriteLine("Enter title");
                            string titleB = Console.ReadLine();
                            select(artistB, titleB);
                            break;
                        case "Delete":
                            Console.WriteLine("Enter artist");
                            string artistC = Console.ReadLine();
                            Console.WriteLine("Enter title");
                            string titleC = Console.ReadLine();
                            delete(artistC, titleC);
                            break;
                        case "Edit":
                            Console.WriteLine("Enter artist name");
                            string artistD = Console.ReadLine();
                            Console.WriteLine("Enter title");
                            string titleD = Console.ReadLine();
                            Console.WriteLine("Enter new Artist name");
                            string _artist = Console.ReadLine();
                            Console.WriteLine("Enter new song title");
                            string _title = Console.ReadLine();
                            edit(artistD, titleD, _artist, _title);
                            break;
                        case "Pop":
                            populate();
                            break;
                        case "Exit":
                            flag = true;
                            break;
                        default:
                            Console.WriteLine("Please enter a valid selection");
                            break;
                    }

                    Console.WriteLine("End of query. \nWould you like to perform another? Yes? No?\n");
                    string continueAnswer = Console.ReadLine();

                    switch (continueAnswer)
                    {
                        case "No":
                            Console.WriteLine("Closing appilcation");
                            flag = true;
                            break;
                        case "Yes":
                            flag = false;
                            break;
                        default:
                            Console.WriteLine("Please enter a valid selection");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            Console.ReadLine();
        }
    }

    interface IActions
    {
        void DisplayName(string name);
        void DisplayDate(DateTime myDate);
    }

    class Artists : IActions
    {
        private string artist;
        private string title;

        public Artists()
        {
            // Default constructor
        }

        static Artists()
        {
            // Static constructor
        }

        public void DisplayName(string name)
        {
            Console.WriteLine($"Hello {name}");
        }

        public void DisplayDate(DateTime myDate)
        {
            Console.WriteLine("Date: {0:d}\nTime: {0:t}", myDate);
        }

        public string Artist
        {
            get { return artist; }
            set
            {
                if (value == "")
                {
                    Console.WriteLine("Please enter a valid value");
                }
                else
                {
                    artist = value;
                }
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (value == "")
                {
                    Console.WriteLine("Please enter a valid string");
                }
                else
                {
                    title = value;
                }
            }
        }

        ~Artists()
        {
            // Destructor
        }
    }

    struct Company 
    {
        private string companyName;
        private string companyAddress;
        private string companyEmail;

        static Company()
        {
            // Static constructor
        }

        public string CompanyName
        {
            get { return companyName; }
            set
            {
                if (value == "")
                {
                    Console.WriteLine("Please enter a valid string");
                }
                else
                {
                    companyName = value;
                }
            }
        }

        public string CompanyAddress
        {
            get { return companyAddress; }
            set
            {
                if (value ==  "")
                {
                    Console.WriteLine("Please enter a valid string");
                }
                else
                {
                    companyAddress = value;
                }
            }
        }

        public string CompanyEmail
        {
            get { return companyEmail; }
            set
            {
                if (value == "")
                {
                    Console.WriteLine("Please enter a valid string");
                }
                else
                {
                    companyEmail = value;
                }
            }
        }
    }

    interface ISQLFunctions 
    {
        void GetAllQuery();
        void DeleteAllQuery();
        void InsertIntoQuery(string artist, string title);
        void SelectByParamQuery(string artist, string title);
        void DeleteByParamQuery(string artist, string title);
        void EditByParamQuery(string artist, string title, string _artist, string _title);
        void PopulateDT();
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
            string connectionString = @"Integrated Security=true;Initial Catalog=MusicStore;Data Source=BOOTCAMP11\SQLEXPRESS;";
            string query = "SELECT * From Music";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("There are no records in the datatable");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            string artist = reader["Artist"].ToString();
                            string title = reader["Title"].ToString();

                            Console.WriteLine($"Artist: {artist}\nTitle: {title}\n");
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.InnerException}");
            }
        }

        public void DeleteAllQuery()
        {
            string connectionString = @"Integrated Security=true;Initial Catalog=MusicStore;Data Source=BOOTCAMP11\SQLEXPRESS;";
            string query = "DELETE FROM Music";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    int result = command.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        Console.WriteLine("There are no records to delete");
                    }
                    else
                    {
                        Console.WriteLine($"{result} rows affected.");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.InnerException}");
            }
        }

        public void InsertIntoQuery(string artist, string title)
        {
            string connectionString = @"Integrated Security=true;Initial Catalog=MusicStore;Data Source=BOOTCAMP11\SQLEXPRESS;";
            string query = "INSERT INTO Music(Artist, Title) VALUES(@artist, @title)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@artist", artist);
                    command.Parameters.AddWithValue("@title", title);

                    int result = command.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        Console.WriteLine("Error adding record to datatable");
                    }
                    else
                    {
                        Console.WriteLine($"{result} rows affected");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.InnerException}");
            }
        }

        public void SelectByParamQuery(string artist, string title)
        {
            string connectionString = @"Integrated Security=true;Initial Catalog=MusicStore;Data Source=BOOTCAMP11\SQLEXPRESS;";
            string query = "SELECT * FROM Music WHERE Artist = @artist AND Title = @title";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@artist", artist);
                    command.Parameters.AddWithValue("@title", title);
                    int result = command.ExecuteNonQuery();

                    SqlDataReader reader = command.ExecuteReader();

                    
                    if (!reader.HasRows || result <= 0)
                    {
                        Console.WriteLine("There are no records in the datatable");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            string Artist = reader["Artist"].ToString();
                            string Title = reader["Title"].ToString();

                            Console.WriteLine($"Artist: {Artist}\nTitle: {Title}\n");

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error {e.InnerException}");
            }
        }

        public void DeleteByParamQuery(string artist, string title)
        {
            string connectionString = @"Integrated Security=true;Initial Catalog=MusicStore;Data Source=BOOTCAMP11\SQLEXPRESS;";
            string query = "DELETE FROM Music WHERE Title = @title AND Artist = @artist";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.AddWithValue("@artist", artist);
                    command.Parameters.AddWithValue("@title", title);

                    int result = command.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        Console.WriteLine("No records to remove");
                    }
                    else
                    {
                        Console.WriteLine($"{result} rows affected");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.InnerException}");
            }
        }

        public void EditByParamQuery(string artist, string title, string _artist, string _title)
        {
            string connectionString = @"Integrated Security=true;Initial Catalog=MusicStore;Data Source=BOOTCAMP11\SQLEXPRESS;";
            string query = "UPDATE Music SET Artist = @_artist, Title = @_title WHERE Artist = @artist AND Title = @title";

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

                    if (result <= 0)
                    {
                        Console.WriteLine("No matching records found");
                    }
                    else
                    {
                        Console.WriteLine($"{result} rows affected");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        public void PopulateDT()
        {
            string connectionString = @"Integrated Security=true;Initial Catalog=MusicStore;Data Source=BOOTCAMP11\SQLEXPRESS;";
            string query = "INSERT INTO Music(Artist, Title) VALUES(@artist, @title)";
            string artist = "";
            string title = "";

            Dictionary<Artists, Company> companyPeople = new Dictionary<Artists, Company>();
            companyPeople.Add(new Artists { Artist = "Audio", Title = "Straight to bad" }, new Company { CompanyName = "Digital Audio Records", CompanyAddress = "18 Long Str. Cape Town", CompanyEmail = "audio@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Maztek", Title = "Gone for days" }, new Company { CompanyName = "Forbidden Recordings", CompanyAddress = "22 Range Rd. Durban", CompanyEmail = "stella@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Goncalo M", Title = "Hardgrooving" }, new Company { CompanyName = "Dying For Digital", CompanyAddress = "90 Jefferey Str. Cape Town", CompanyEmail = "goncalom@gmail.com" });
            companyPeople.Add(new Artists { Artist = "F-Tek", Title = "Minimal" }, new Company { CompanyName = "Dream Digital", CompanyAddress = "45 Hill Str. Johannesburg", CompanyEmail = "ftek@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Bad Signal", Title = "Heart beat" }, new Company { CompanyName = "Makeshift Beats", CompanyAddress = "32 Green Rd. Johannesburg", CompanyEmail = "bad@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Darmec", Title = "Doof Doof" }, new Company { CompanyName = "Bad Taste Audio", CompanyAddress = "23 Jippe Str. Johannesburg", CompanyEmail = "darmec@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Sputnik", Title = "Greener pastures" }, new Company { CompanyName = "Just Done", CompanyAddress = "45 Franklyn Avenue", CompanyEmail = "sput@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Optiv", Title = "Lost generation" }, new Company { CompanyName = "Split It Audio", CompanyAddress = "23 Krune Rd. Cape Town", CompanyEmail = "optiv@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Tessa", Title = "Feels like daydreaming" }, new Company { CompanyName = "Deaf Recordings", CompanyAddress = "12 Ben Drive Johannesburg", CompanyEmail = "tess@gmail.com" });
            companyPeople.Add(new Artists { Artist = "David", Title = "Not so good" }, new Company { CompanyName = "Tell It Recordings", CompanyAddress = "67 Weast Aveneue Cape town", CompanyEmail = "davidg@gmail.com" });
            companyPeople.Add(new Artists { Artist = "David Tamessi", Title = "Doof Doof" }, new Company { CompanyName = "Source Audio", CompanyAddress = "23 June Str. Cape Town", CompanyEmail = "dt@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Jim Joelson", Title = "Seems like dawn" }, new Company { CompanyName = "Lion Audio", CompanyAddress = "45 Gregory Drive Johannesburg", CompanyEmail = "jj@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Henry the Pope", Title = "Tears at dawn" }, new Company { CompanyName = "Digital Dreams", CompanyAddress = "45 Long Str. Cape Town", CompanyEmail = "henry@gmail.com" });
            companyPeople.Add(new Artists { Artist = "David Tamessi", Title = "YOLO" }, new Company { CompanyName = "Tech Records", CompanyAddress = "112  Hill Rd. Johannesburg", CompanyEmail = "david@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Yolandi", Title = "Dreams on tap" }, new Company { CompanyName = "Lion Audio", CompanyAddress = "45 Gregory Drive Johannesburg", CompanyEmail = "yol@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Gill Fisher", Title = "Here we go now" }, new Company { CompanyName = "Pole Position Records", CompanyAddress = "12 Peter Rd. Johannesburg", CompanyEmail = "gill@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Paulo", Title = "A1" }, new Company { CompanyName = "Real records", CompanyAddress = "90 Gutter Rd. Johannesburg", CompanyEmail = "paulo@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Kenneth", Title = "Nowhere" }, new Company { CompanyName = "Final State Records", CompanyAddress = "342 Rivonia Rd. Johannesburg", CompanyEmail = "kenny@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Jackie Knox", Title = "Knox you out" }, new Company { CompanyName = "Lone Records", CompanyAddress = "223 Sandton Drive Johannesburg", CompanyEmail = "jk@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Liam Fisher", Title = "Know your here" }, new Company { CompanyName = "Null Records", CompanyAddress = "45 Tall Str. Johannesburg", CompanyEmail = "liam@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Mike Bully", Title = "Living Large" }, new Company { CompanyName = "Null Records", CompanyAddress = "345 June Str. Cape Town", CompanyEmail = "mikeb@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Neil Diamon", Title = "Sweet Caroline" }, new Company { CompanyName = "Rough Records", CompanyAddress = "887 Sandton Drive Johannesburg", CompanyEmail = "neil@gmail.com" });
            companyPeople.Add(new Artists { Artist = "Jack Jackson", Title = "Hard Banger" }, new Company { CompanyName = "Done Records", CompanyAddress = "12 Hendrick Str. Johannesburg", CompanyEmail = "jack@gmail.com" });

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(query, conn);

                    foreach (var person in companyPeople)
                    {
                        artist = person.Key.Artist;
                        title = person.Key.Title;
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@artist", artist);
                        int result = command.ExecuteNonQuery();
                        command.Parameters.Clear();
                        Console.WriteLine($"{result} rows affected");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
