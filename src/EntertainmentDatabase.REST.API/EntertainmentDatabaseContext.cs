﻿using EntertainmentDatabase.REST.API.ContextConfiguration;
using EntertainmentDatabase.REST.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieActors = EntertainmentDatabase.REST.API.ContextConfiguration.MovieActors;

namespace EntertainmentDatabase.REST.API
{
    public class EntertainmentDatabaseContext : DbContext
    {
        private const string Production = "Production";
        private const string Development = "Development";
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostingEnvironment hostingEnvironment;

        public EntertainmentDatabaseContext(IConfigurationRoot configurationRoot, IHostingEnvironment hostingEnvironment)
        {
            this.configurationRoot = configurationRoot;
            this.hostingEnvironment = hostingEnvironment;
        }

        public DbSet<Movie> Movies;

        public DbSet<Actor> Actors;

        public DbSet<Domain.Entities.MovieActors> ActorMovies;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                this.hostingEnvironment.IsDevelopment()
                    ? this.configurationRoot.GetConnectionString(EntertainmentDatabaseContext.Development)
                    : this.configurationRoot.GetConnectionString(EntertainmentDatabaseContext.Production));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new MovieConfiguration()
                .Configure(modelBuilder);

            new MovieFileConfiguration()
                .Configure(modelBuilder);

            new ActorConfiguration()
                .Configure(modelBuilder);

            new MovieActors()
                .Configure(modelBuilder);

            modelBuilder.DisableCascadeDelete();
        }
    }
}
