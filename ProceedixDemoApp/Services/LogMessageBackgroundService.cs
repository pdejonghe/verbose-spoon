using AutoMapper;
using ProceedixDemoApp.DTOs;
using ProceedixDemoApp.Models;
using ProceedixDemoApp.Repositories;

namespace ProceedixDemoApp.Services
{
    public class LogMessageBackgroundService : BackgroundService
    {
        private readonly IMapper _mapper;
        private readonly IBackgroundQueue<LogMessageDto[]> _queue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LogMessageBackgroundService(IMapper mapper, IBackgroundQueue<LogMessageDto[]> queue, IServiceScopeFactory serviceScopeFactory)
        {
            _mapper = mapper;
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await BackgroundProcessing(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);

                var logMessages = _queue.Dequeue();

                if (logMessages == null)
                {
                    continue;
                }

                using var scope = _serviceScopeFactory.CreateScope();
                var logMessageRepository = scope.ServiceProvider.GetRequiredService<ILogMessageRepository>();

                // Map LogMessageDto[] to LogMessage[] using AutoMapper
                var logMessageEntities = _mapper.Map<LogMessageDto[], LogMessage[]>(logMessages);

                // Save LogMessage[] to database
                await logMessageRepository.Create(logMessageEntities);
            }
        }
    }
}