using Project.Data; 
using Project.Models;
using Project.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Project.Models.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies.FirstOrDefault(m => m.MovieId == id);
        }

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public void Update(Movie movie)
        {
            var existing = _context.Movies.FirstOrDefault(m => m.MovieId == movie.MovieId);
            if (existing != null)
            {
                existing.Title = movie.Title;
                existing.Genre = movie.Genre;
                existing.Price = movie.Price;
                existing.PosterUrl = movie.PosterUrl;
                existing.TicketsAvailable = movie.TicketsAvailable;
                _context.SaveChanges(); 
            }
        }


        public void Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }
    }
}
