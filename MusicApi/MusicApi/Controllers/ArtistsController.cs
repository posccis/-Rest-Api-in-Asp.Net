﻿using Microsoft.AspNetCore.Http;
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
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromForm] Artist artist)
        {
            var imageUrl = await FileHelper.UploadImage(artist.Image);
            artist.ImageUrl = imageUrl;
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetArtists(int pageNumber, int pageSize)
        {
            var artists = await (from artist in _dbContext.Artists
                                 select new
                                 {
                                     Id = artist.Id,
                                     Name = artist.Name,
                                     ImageUrl = artist.ImageUrl
                                 }).ToListAsync();
            return Ok(artists.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistsDetails(int artistId)
        {
            var artistDetails = await _dbContext.Artists.Where(a=> a.Id == artistId).Include(a => a.Songs).ToListAsync();
            return Ok(artistDetails);
        }
        

    }
}
