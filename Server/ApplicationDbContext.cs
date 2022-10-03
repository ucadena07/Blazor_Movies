﻿using BlazorMovies.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorMovies.Server
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesActors>().HasKey(it => new { it.MovieId, it.PersonId});
            modelBuilder.Entity<MoviesGenre>().HasKey(it => new { it.MovieId, it.GenreId});
            base.OnModelCreating(modelBuilder); 
        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenre> MoviesGenres { get; set; }
    }
}
