using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Repositories.interfaces;

namespace Repositories;
public class MongoDbSnippetRepository : ISnippetRepository
{
    private readonly IMongoCollection<Snippet> _snippetCollection;

    public MongoDbSnippetRepository(IMongoDatabase database)
    {
        _snippetCollection = database.GetCollection<Snippet>("snippets");
    }

    public async Task<string> CreateAsync(Snippet snippet)
    {
        if (snippet.Text != null)
        {
        snippet.Slug = GenerateSlug(snippet.Text);
        snippet.PreviousVersions?.Add(snippet.Text);
        snippet.CreatedAt = DateTime.Now;

        await _snippetCollection.InsertOneAsync(snippet);

        return snippet.Slug;
        }
        return "string cannot be empty.";
    }

    public async Task<Snippet> GetBySlugAsync(string slug)
    {
        return await _snippetCollection.Find(s => s.Slug == slug).FirstOrDefaultAsync();
    }

    public async Task<string> UpdateAsync(string slug, Snippet updatedSnippet)
    {   
        var existingSnippet = await _snippetCollection.FindOneAndUpdateAsync(
            s => s.Slug == slug,
            Builders<Snippet>.Update
            .Push(s => s.PreviousVersions, updatedSnippet.Text)
                .Set(s => s.Text, updatedSnippet.Text)
                .Set(s => s.EditedAt, DateTime.Now));

        return existingSnippet != null ? slug : null;
    }

    private string GenerateSlug(string text)
    {
        
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var slugBuilder = new StringBuilder();
            for (var i = 0; i < 8; i++)
            {
                var index = random.Next(chars.Length);
                slugBuilder.Append(chars[index]);
            }
            return slugBuilder.ToString();
    }
}
