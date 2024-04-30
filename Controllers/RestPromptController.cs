using DotnetGeminiSDK.Client.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace GeminiPromptGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestPromptController : ControllerBase
    {
        private string apiKey = "Your-key-for-test";
        public RestPromptController()
        { 
        }

        [HttpPost("PromptText")]
        public async Task<IActionResult> PromptText(string text)
        {
            string output = await SendRequestAndGetResponse(text);

            output = output.Replace("\\n", Environment.NewLine)
                           .Replace("\n", "")
                           .Replace("**", "");
            if (output == null)
            {
                return NotFound();
            }
            return Ok(output);
        }

        private async Task<string> SendRequestAndGetResponse(string userInput)
        {
            string jsonBody = $@"{{
                ""contents"": [
                    {{
                        ""role"": """",
                        ""parts"": [
                            {{
                                ""text"": ""{userInput}""
                            }}
                        ]
                    }}
                ],
                ""generationConfig"": {{
                    ""temperature"": 0.9,
                    ""topK"": 50,
                    ""topP"": 0.95,
                    ""maxOutputTokens"": 4096,
                    ""stopSequences"": []
                }},
                ""safetySettings"": [

                ]
            }}
            ";

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.0-pro:generateContent?key={apiKey}");
            request.Content = new StringContent(jsonBody, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody.Substring(responseBody.IndexOf("\"text\": \"") + 9, responseBody.IndexOf("\"", responseBody.IndexOf("\"text\": \"") + 10) - responseBody.IndexOf("\"text\": \"") - 9);
            }
            else
            {
                return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
            }
        }
    }
}
