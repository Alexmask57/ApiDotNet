namespace ApiDotNet.Config;

public class BookStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string Server { get; set; } = null!;
    public int Port { get; set; }
    public string DatabaseName { get; set; } = null!;

    public string BooksCollectionName { get; set; } = null!;
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
}