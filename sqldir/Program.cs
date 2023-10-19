using Microsoft.Data.Sqlite;


bool stopper = false;
string directoryinfo="";
string path = "C:\\GameCenter\\";
int score = 0; 
string[] file = new string[100];
string[] dir = new string[100];
int g = 0;


Search();

void ListDir()
{
    if (Directory.Exists(path))
    {
        string[] dirs = Directory.GetDirectories(path);
        Array.Resize(ref dir, dirs.Length);
        foreach (string s in dirs)
        {
            directoryinfo += s+"\n";
            dir[g] = s;
            g++;
        }
        score++;
    }
    g = 0;
}

void ListCreate()
{
    string sqlDirectoryInfo = String.Format("UPDATE DirectoryLog SET ID=1 WHERE DirectoryInfo=('{0}')", directoryinfo);
    string sqlPath = String.Format("UPDATE DirectoryLog SET ID=1 WHERE Path=('{0}')", path);
    using (var connection = new SqliteConnection("Data Source=usersdata.db"))
    {
        connection.Open();

        SqliteCommand command = new SqliteCommand(sqlDirectoryInfo, connection);

        int number = command.ExecuteNonQuery();

    }
    using (var connection = new SqliteConnection("Data Source=usersdata.db"))
    {
        connection.Open();

        SqliteCommand command = new SqliteCommand(sqlPath, connection);

        int number = command.ExecuteNonQuery();
    }
}
void DirectoryPath()
{
    using (var connection = new SqliteConnection("Data Source=usersdata.db"))
    {
        connection.Open();

        SqliteCommand command = new SqliteCommand();
        command.Connection = connection;
        command.CommandText = String.Format("INSERT INTO DirectoryLog (Path) VALUES ('{0}')", path);
        int number = command.ExecuteNonQuery();

        Console.WriteLine($"В таблицу Users добавлено объектов: {number}");
    }
}
void ListClear()
{
    string sqlExpression = "DELETE  FROM DirectoryLog WHERE Name='1'";
    using (var connection = new SqliteConnection("Data Source=usersdata.db"))
    {
        connection.Open();

        SqliteCommand command = new SqliteCommand(sqlExpression, connection);

        int number = command.ExecuteNonQuery();
    }
}
void ListFiles()
{


    if (Directory.Exists(path))
    {
        string[] dirs = Directory.GetFiles(path);
        Array.Resize(ref file, dirs.Length);
        foreach (string s in dirs)
        {
            directoryinfo += s + "\n";
            file[g] = s;
            g++;

        }
    }
    g = 0;

}
async void Search()
{
    DirectoryPath();
    ListDir();
    ListFiles();
    ListCreate();
    stopper = false;
    while (stopper == false)
    {
        string[] dirs = Directory.GetDirectories(path);
        string[] files = Directory.GetFiles(path);
        if (dir.Length != dirs.Length || file.Length != files.Length)
        {
            //ListClear();
            ListDir();
            ListFiles();
            ListCreate();
        }
        foreach (string s in dirs)
        {
            if (dir[g] != s)
            {
                //ListClear();
                g = 0;
                ListDir();
                ListFiles();
                ListCreate(); ;
                break;
            }
            g++;

        }
        g = 0;
        foreach (string s in files)
        {
            if (file[g] != s)
            {
                //ListClear();
                g = 0;
                ListDir();
                ListFiles();
                ListCreate();
                break;
            }
            g++;

        }
        g = 0;
    }
}