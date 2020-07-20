using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnonymousWebApi.Data.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class AnonymousDBController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository repository;

        public AnonymousDBController(TRepository repository)
        {
            this.repository = repository;
        }


        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            return await repository.GetAll().ConfigureAwait(false);
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> Get(int id)
        {
            var movie = await repository.Get(id).ConfigureAwait(false);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, TEntity movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }
            await repository.Update(movie).ConfigureAwait(false);
            return NoContent();
        }

        // POST: api/[controller]
        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity movie)
        {
            await repository.Add(movie).ConfigureAwait(false);
            return CreatedAtAction("Get", new { id = movie.Id }, movie);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TEntity>> Delete(Guid id)
        {
            var movie = await repository.Delete(id).ConfigureAwait(false);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }
    }
}