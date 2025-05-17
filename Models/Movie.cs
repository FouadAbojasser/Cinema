using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace Cinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }=string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string Trailer { get; set; } = string.Empty;
        public DateOnly? ProductionDate { get; set; }
        public DateOnly? ReleaseDate { get; set; } 
        public string Duration { get; set; } = string.Empty;
        public double Rate { get; set; }
        public string Country { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string AgeRating { get; set; } = string.Empty;
        public int DirectorId { get; set; }
        public Director Director { get; set; } = null!;
        public ICollection<Genre> Genres { get; set; } = [];
        public ICollection<Image> Images { get; set; } = [];
        public ICollection<Actor> Actors { get; set; } = [];
        public ICollection<MovieTheater> MovieTheaters { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];

    }

}
