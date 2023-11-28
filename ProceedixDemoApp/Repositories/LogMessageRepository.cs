using Microsoft.EntityFrameworkCore;
using ProceedixDemoApp.Data;
using ProceedixDemoApp.Models;

namespace ProceedixDemoApp.Repositories
{
    public class LogMessageRepository : ILogMessageRepository
    {
        private readonly ProceedixDemoAppDbContext _context;

        public LogMessageRepository(ProceedixDemoAppDbContext context)
        {
            _context = context;
        }

        public async Task Create(LogMessage[] logMessages)
        {
            // Set up retry logic in case of concurrency exceptions
            var retryCount = 0;
            var maxRetryCount = 3;
            var delay = 1000;

            while (true)
            {
                try
                {
                    // Check if logMessages is null or empty
                    if (logMessages == null || logMessages.Length == 0)
                    {
                        return;
                    }

                    // Get a list of application names from the logMessages array
                    var applicationNames = logMessages.Select(x => x.Application?.Name).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                    // Query the database for applications with matching names
                    var applications = await _context.Applications.Where(e => applicationNames.Contains(e.Name)).ToListAsync();

                    // Loop through each logMessage and update its Application property
                    foreach (var logMessage in logMessages)
                    {
                        if (!string.IsNullOrWhiteSpace(logMessage?.Application?.Name))
                        {
                            // Find the application with a matching name
                            var application = applications.Find(y => y.Name == logMessage.Application.Name);

                            // If the application doesn't exist, create a new one
                            if (application == null)
                            {
                                application = new Application { Name = logMessage.Application.Name };
                                _context.Applications.Add(application);
                                applications.Add(application);
                            }

                            // Update the logMessage's Application property
                            logMessage.Application = application;
                        }
                    }

                    // Add the logMessages to the database and save changes
                    await _context.LogMessages.AddRangeAsync(logMessages);
                    await _context.SaveChangesAsync();

                    break;
                }
                catch (DbUpdateConcurrencyException) when (retryCount < maxRetryCount)
                {
                    // If a concurrency exception occurs, retry up to maxRetryCount times with a delay of delay milliseconds
                    retryCount++;
                    await Task.Delay(delay);
                }
            }
        }
    }
}