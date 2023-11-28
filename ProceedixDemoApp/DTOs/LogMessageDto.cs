using System.Text.Json.Serialization;

namespace ProceedixDemoApp.DTOs
{
    public class LogMessageDto
    {
        [JsonPropertyName("log_date")]
        public long LogDate { get; set; }

        public string Application { get; set; }

        public string Message { get; set; }
    }
}