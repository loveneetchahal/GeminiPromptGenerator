﻿using GeminiPromptGenerator.Dtos;
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
    public class RestPromptController : ControllerBase
    {
        //Download google installer to login
        //https://dl.google.com/dl/cloudsdk/channels/rapid/GoogleCloudSDKInstaller.exe
        // How to setup
        //https://cloud.google.com/docs/authentication/provide-credentials-adc#local-dev
        const string ProjectId = "engaged-plasma-421810";
        const string Location = "us-central1";
        const string AiPlatformUrl = $"https://{Location}-aiplatform.googleapis.com";
        const string ModelId = "gemini-pro";
        const string EndpointUrl = $"{AiPlatformUrl}/v1/projects/{ProjectId}/locations/{Location}/publishers/google/models/{ModelId}:streamGenerateContent";
        public RestPromptController()
        { 

        }

        [HttpPost("PromptText")]
        public async Task<IActionResult> PromptText(string text)
        {
            string payload = GeneratePayload(text);
            string response = await SendRequest(payload);
            var geminiResponses = JsonConvert.DeserializeObject<List<GeminiResponse>>(response);

            string fullText = string.Join("", geminiResponses
                .SelectMany(co => co.Candidates)
                .SelectMany(c => c.Content.Parts)
                .Select(p => p.Text));

            if (fullText == null)
            {
                return NotFound();
            }
            return Ok(fullText);
        }

        private static string GeneratePayload(string text)
        {
            var payload = new
            {
                contents = new
                {
                    role = "USER",
                    parts = new
                    {
                        text = text
                    }
                },
                generation_config = new
                {
                    temperature = 0.4,
                    top_p = 1,
                    top_k = 32,
                    max_output_tokens = 2048
                }
            };
            return JsonConvert.SerializeObject(payload);
        }

        private async static Task<string> SendRequest(string payload)
        {
            GoogleCredential credential = GoogleCredential.GetApplicationDefault();
            var handler = credential.ToDelegatingHandler(new HttpClientHandler());
            using HttpClient httpClient = new(handler);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await httpClient.PostAsync(EndpointUrl,
                new StringContent(payload, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
