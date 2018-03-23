using Movies.API.Models;
using MovieStore.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Movies.API.Data
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;

        public Seed(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SeedAll()
        {
            try
            {
                SeedMovies();
                SeedUsers();
            }
            catch (Exception)
            {
            }

        }
        public void SeedUsers()
        {
            if (!_context.Useri.Any())
            {
                byte[] AdminPasswordHash, AdminPasswordSalt;
                CreatePasswordHash("admin", out AdminPasswordHash, out AdminPasswordSalt);
                var admin = new User
                {
                    Username = "Admin",
                    PasswordHash = AdminPasswordHash,
                    PasswordSalt = AdminPasswordSalt,
                    Role = "admin"
                };

                byte[] UserPasswordHash, UserPasswordSalt;
                CreatePasswordHash("user1", out UserPasswordHash, out UserPasswordSalt);
                var user = new User
                {
                    Username = "User",
                    PasswordHash = UserPasswordHash,
                    PasswordSalt = UserPasswordSalt,
                    Role = ""
                };
                _context.Useri.AddRange(admin, user);
                _context.SaveChanges();
            }

        }
        private void SeedMovies()
        {
            if (!_context.Movies.Any())
            {
                var movieData = System.IO.File.ReadAllText("Data/moviesForSeed.json");
                var movies = JsonConvert.DeserializeObject<List<Movie>>(movieData);
                _context.Movies.AddRange(movies);
                _context.SaveChanges();
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}