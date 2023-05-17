using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Repositories.interfaces
{
    public interface ISnippetRepository
    {
        Task<Snippet> GetBySlugAsync(string slug);
        Task<string> CreateAsync(Models.Snippet snippet);
        Task<string> UpdateAsync(string slug, Snippet updatedSnippet);
    }
}