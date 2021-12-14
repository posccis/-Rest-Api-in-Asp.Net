using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name can't be empty!")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        [Required(ErrorMessage ="Artist ID can't be empty!")]
        public int ArtistId { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }

        public ICollection<Song> Songs { get; set; }
    }
}
