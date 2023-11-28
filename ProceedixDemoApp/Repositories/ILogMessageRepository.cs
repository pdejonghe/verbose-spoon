using ProceedixDemoApp.Models;

namespace ProceedixDemoApp.Repositories
{
    public interface ILogMessageRepository
    {
        Task Create(LogMessage[] logMessages);
    }
}