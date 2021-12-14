using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Post([FromForm] Album album) 
        {
            var imageUrl = await FileHelper.UploadImage(album.Image);
            album.ImageUrl = imageUrl;

            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]

        public async Task<IActionResult> GetAlbums(int pageNumber, int pageSize)
        {
            var albums = await (from album in _dbContext.Albums
                                select new
                                {
                                    Id = album.Id,
                                    Name = album.Name,
                                    Image = album.ImageUrl,
                                    ArtistId = album.ArtistId 
                                }
                                ).ToListAsync();
            return Ok(albums.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        [HttpGet("[action]")]

        public async Task<IActionResult> AlbumsDetails(int AlbumId)
        {
            var albumDetails = await _dbContext.Albums.Where(a => a.Id == AlbumId).Include(a => a.Songs).ToListAsync();
            return Ok(albumDetails);
        }
    }
}
