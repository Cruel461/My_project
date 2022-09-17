using ITWitor.Data;
using ITWitor.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using System.Diagnostics;
using System.Text;

using System.Drawing;

using System.Net;
using System.Net.Http;
using System.Net.Mime;

namespace ITWitor.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("api/import")]
        public async Task<JsonResult> Import([FromForm] string? json)
        {
            ApiResponse response = new();
            var apiKey = ControllerContext.HttpContext.Request.Headers.Where(x => x.Key == "x-api-key").FirstOrDefault().Value;
            if (string.IsNullOrEmpty(json))
            {
                response.Message = "json is null";
            }
            else if (string.IsNullOrEmpty(apiKey))
            {
                response.Message = "x-api-key header not found";
            }
            else if (apiKey != "RTIWhgQKyxhDpXHS7VJ81T5k4Gzf9JemsGbfFWg7pzgVpIGFIc9iGNIZT7SgFD6K")
            {
                response.Message = "wrong x-api-key";
            }
            else
            {
                var filePath = Startup.Environment.WebRootPath + @"\logs\" + DateTime.Now.ToLongTimeString().Replace(":", "_") + ".json";


                using (FileStream fstream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    byte[] buffer = Encoding.Default.GetBytes(json);
                    await fstream.WriteAsync(buffer, 0, buffer.Length);
                }
                response.Message = json;
                response.Success = true;
            }

            return new JsonResult(response);
        }
    }

    public class ApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}