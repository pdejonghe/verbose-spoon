using AutoMapper;
using ProceedixDemoApp.DTOs;
using ProceedixDemoApp.Models;
using System.Text.RegularExpressions;

namespace ProceedixDemoApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map LogMessageDto to LogMessage
            CreateMap<LogMessageDto, LogMessage>()
                // Execute MapLogLevelAndMessage method before mapping
                .BeforeMap((src, dest) => MapLogLevelAndMessage(src.Message, dest))
                // Ignore LogLevel and Message properties (mapped in BeforeMap)
                .ForMember(dest => dest.LogLevel, act => act.Ignore())
                .ForMember(dest => dest.Message, act => act.Ignore())
                // Map Date property
                .ForMember(dest => dest.Date, act => act.MapFrom(src => UnixEpochTimestampToDateTime(src.LogDate)))
                // Map Application property
                .ForMember(dest => dest.Application, act => act.MapFrom(src => new Application() { Name = src.Application }))
            ;
        }

        // Helper method to extract log level and message from log message
        private static (string logLevel, string message) GetLogLevelAndMessage(string logMessage)
        {
            // Define regex pattern to match log level and message
            var regex = new Regex(@"^\[(?<logLevel>[^\]]+)\]\s*(?<message>.*)$");
            var match = regex.Match(logMessage);

            if (match.Success)
            {
                // Return log level and message
                return (match.Groups["logLevel"].Value, match.Groups["message"].Value);
            }

            // Return empty strings if no match found
            return (string.Empty, logMessage);
        }

        // Set LogLevel and Message properties of LogMessage object
        private static void MapLogLevelAndMessage(string originalMessage, LogMessage dest)
        {
            // Trim original message
            originalMessage = originalMessage.Trim();

            // Set default LogLevel and Message
            dest.LogLevel = Models.LogLevel.Info;
            dest.Message = originalMessage;

            if (!originalMessage.StartsWith('['))
            {
                // Return if no log level found
                return;
            }

            // Extract log level and message
            (var logLevel, var message) = GetLogLevelAndMessage(originalMessage);

            if (!string.IsNullOrWhiteSpace(logLevel) && Enum.TryParse(typeof(Models.LogLevel), logLevel, true, out var logLevelEnum))
            {
                // Set LogLevel if valid
                dest.LogLevel = (Models.LogLevel)logLevelEnum;
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                // Set Message if not empty
                dest.Message = message;
            }
        }

        // Convert Unix epoch timestamp to DateTime
        private static DateTime UnixEpochTimestampToDateTime(long logDate)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(logDate);
            return dateTimeOffset.UtcDateTime;
        }
    }
}