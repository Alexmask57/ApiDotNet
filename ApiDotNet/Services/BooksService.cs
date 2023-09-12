using System.Security;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using ApiDotNet.Config;
using ApiDotNet.Models;

namespace ApiDotNet.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var settings = new MongoClientSettings()
        {
            Server = new MongoServerAddress(bookStoreDatabaseSettings.Value.Server, bookStoreDatabaseSettings.Value.Port),
            Credential = MongoCredential.CreateCredential(bookStoreDatabaseSettings.Value.DatabaseName,bookStoreDatabaseSettings.Value.User, bookStoreDatabaseSettings.Value.Password)
        };
        var mongoClient = new MongoClient(settings);

        var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Book>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);
}