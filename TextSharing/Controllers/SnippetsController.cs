using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories.interfaces;

namespace TextSharing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SnippetsController : ControllerBase
    {
        private readonly ISnippetRepository _snippetRepository;

        public SnippetsController(ISnippetRepository snippetRepository)
        {
            _snippetRepository = snippetRepository;
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult<Snippet>> Get(string slug)
        {
            var snippet = await _snippetRepository.GetBySlugAsync(slug);

            if (snippet == null)
            {
                return NotFound();
            }

            return Ok(snippet);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] Snippet snippet)
        {
            var slug = await _snippetRepository.CreateAsync(snippet);

            if (slug == null)
            {
                return BadRequest("Failed to create snippet.");
            }
            

            return CreatedAtAction(nameof(Get), new { slug }, slug);
        }

        [HttpPut("edit/{slug}")]
        public async Task<ActionResult<string>> Put(string slug, [FromBody] Snippet updatedSnippet)
        {
            var result =  await _snippetRepository.UpdateAsync(slug, updatedSnippet);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(slug);
        }

        [HttpGet("{slug}/versions")]
        public async Task<IActionResult> GetVersions(string slug)
        {
            var existingSnippet = await _snippetRepository.GetBySlugAsync(slug);
            if (existingSnippet == null)
            {
                return NotFound();
            }

            return Ok(existingSnippet.PreviousVersions);
        }
    }
}