﻿using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace InCinema.Models.Movies;

public class MovieCreate
{
    [Required] public string Name { get; set; }
    [Required] public DateTime ReleaseDate { get; set; }
    [Required] [Min(0)] public decimal Budget { get; set; }
    [Required] public TimeSpan Time { get; set; }
}