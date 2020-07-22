using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace ImportAsapUserToTessituraService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Uri _serverSettings;
        public Worker(ILogger<Worker> logger, IOptions<ServerSettings> serverSettings)
        {
            _logger = logger;
            _serverSettings = serverSettings.Value.ServerAddress;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
	            var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{_serverSettings}api/Importapi", stoppingToken);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation($"Daily run completed successfully at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
