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
        private readonly Uri _serverAddress;
        public Worker(ILogger<Worker> logger, IOptions<ServerSettings> serverSettings)
        {
            _logger = logger;
            _serverAddress = serverSettings.Value.ServerAddress;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
	            using (var client = new HttpClient())
	            {
		            client.BaseAddress = _serverAddress;
		            var response = await client.GetAsync("api/Importapi", stoppingToken);
		            response.EnsureSuccessStatusCode();
	            }
               
                _logger.LogInformation($"Daily run completed successfully at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
