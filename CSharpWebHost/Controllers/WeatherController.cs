using System;
using Microsoft.AspNetCore.Mvc;
using CSharpWebHost.Models;
using CSharpWebHost.Data;

namespace CSharpWebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(WeatherStore.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var weather = WeatherStore.GetById(id);
            if (weather == null)
                return NotFound();
            return Ok(weather);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Weather weather)
        {
            weather.UpdateTime = DateTime.Now;
            WeatherStore.Add(weather);
            return CreatedAtAction(nameof(Get), new { id = weather.Id }, weather);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Weather weather)
        {
            if (id != weather.Id)
                return BadRequest();
                
            var existingWeather = WeatherStore.GetById(id);
            if (existingWeather == null)
                return NotFound();

            weather.UpdateTime = DateTime.Now;
            WeatherStore.Update(weather);
            return Ok(weather);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var weather = WeatherStore.GetById(id);
            if (weather == null)
                return NotFound();

            WeatherStore.Delete(id);
            return NoContent();
        }
    }
}
