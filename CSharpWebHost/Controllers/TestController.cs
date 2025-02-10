using Microsoft.AspNetCore.Mvc;

namespace CSharpWebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("GET请求成功");
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return Ok($"POST请求成功: {value}");
        }

        [HttpPut]
        public IActionResult Put([FromBody] string value)
        {
            return Ok($"PUT请求成功: {value}");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok($"DELETE请求成功: {id}");
        }
    }
}
