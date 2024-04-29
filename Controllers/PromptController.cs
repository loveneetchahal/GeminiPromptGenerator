using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model.Response;
using GeminiPromptGenerator.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeminiPromptGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromptController : ControllerBase
    {
        private readonly IGeminiClient _geminiClient;
        public PromptController(IGeminiClient geminiClient)
        {
            _geminiClient = geminiClient;
        }
        [HttpPost("PromptText")]
        public async Task<IActionResult> PromptText(string text)
        {
            //https://console.cloud.google.com/apis/api/generativelanguage.googleapis.com/metrics?project=engaged-plasma-421810
            var response = await _geminiClient.TextPrompt(text);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost("PromptImage")]
        public async Task<IActionResult> PromptImage([FromBody] ImageRequestDto request)
        {
            var response = await _geminiClient.ImagePrompt(request.Message, request.Base64Img, request.MimeType);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
