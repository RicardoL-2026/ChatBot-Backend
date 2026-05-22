using System.Text;
using UglyToad.PdfPig;

namespace Backend_ChatBot.Services
{
    public class ResumeTextExtractorService
    {
        public async Task<string> ExtractTextAsync(byte[] fileBytes, string contentType)
        {
            if (contentType == "text/plain")
            {
                return System.Text.Encoding.UTF8.GetString(fileBytes);
            }

            if (contentType == "application/pdf")
            {
                using var memoryStream = new MemoryStream(fileBytes);

                using var document = PdfDocument.Open(memoryStream);

                var text = new StringBuilder();

                foreach (var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }

                return text.ToString();
            }

            return string.Empty;
        }
    }
}
