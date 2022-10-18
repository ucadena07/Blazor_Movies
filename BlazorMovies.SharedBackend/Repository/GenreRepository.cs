using BlazorMovies.Shared.Entities;
using BlazorMovies.Shared.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.SharedBackend.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;
        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> GetGenre(int id)
        {
            return await _context.Genres.FindAsync(id);
         
        }

        public async Task<List<Genre>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }
        public async Task CreateGenre(Genre genre)
        {
            _context.Add(genre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGenre(int Id)
        {

            var genre = await GetGenre(Id);
            _context.Remove(genre);
            await _context.SaveChangesAsync();
       
        }

    

        public async Task UpdateGenre(Genre genre)
        {
            _context.Attach(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
      
        }
    }
}
