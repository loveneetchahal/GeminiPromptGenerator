using DotnetGeminiSDK.Client.Interfaces;
using GeminiPromptGenerator.Dtos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GeminiPromptGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestTreePromptController : ControllerBase
    {

        private readonly IGeminiClient _geminiClient;
        public RestTreePromptController(IGeminiClient geminiClient)
        {
            _geminiClient = geminiClient;
        }
        [HttpGet("PromptText/{text}/noOfChapters/materialsPerChapter")]
        public async Task<IActionResult> PromptText(string text, int numChapters, int materialsPerChapter)
        {
            var prompt = $"Create a {text} course with {numChapters} chapters, each containing {materialsPerChapter} materials.";

            var response = await _geminiClient.TextPrompt(prompt);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
