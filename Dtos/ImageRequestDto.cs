using DotnetGeminiSDK.Model;

namespace GeminiPromptGenerator.Dtos
{
    public class ImageRequestDto
    {
        public string Message { get; set; }
        public string Base64Img { get; set; }
        public ImageMimeType MimeType { get; set; }
    }
}
