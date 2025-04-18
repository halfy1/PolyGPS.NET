using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SatelliteTracker.Backend.Models;
using SatelliteTracker.Backend.Repositories.Interfaces;
using SatelliteTracker.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace SatelliteTracker.Backend.Services
{
    public class GpsDataBackgroundService : BackgroundService
    {
        private readonly ILogger<GpsDataBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly WebSocketConnectionManager _wsManager;

        public GpsDataBackgroundService(
            ILogger<GpsDataBackgroundService> logger,
            IServiceProvider serviceProvider,
            WebSocketConnectionManager wsManager)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _wsManager = wsManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var parser = scope.ServiceProvider.GetRequiredService<INmeaParserService>();
                        var repository = scope.ServiceProvider.GetRequiredService<ISatelliteDataRepository>();

                        var nmeaMessages = GenerateSampleNmeaMessages();

                        foreach (var message in nmeaMessages)
                        {
                            var data = await parser.ParseNmeaMessage(message);
                            if (data != null)
                            {
                                await repository.AddSatelliteDataAsync(data);
                                await BroadcastData(data);
                            }
                        }
                    }

                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in GPS data background service");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        private async Task BroadcastData(SatelliteData data)
        {
            var sockets = _wsManager.GetAllSockets();
            if (sockets.Count == 0) return;

            var json = JsonConvert.SerializeObject(data);
            var buffer = Encoding.UTF8.GetBytes(json);

            foreach (var socket in sockets)
            {
                if (socket.Value.State == WebSocketState.Open)
                {
                    await socket.Value.SendAsync(
                        new ArraySegment<byte>(buffer),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
            }
        }

        private List<string> GenerateSampleNmeaMessages()
        {
            return new List<string>
            {
                "$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47",
                "$GPGSV,2,1,08,01,40,083,46,02,17,308,41,03,07,344,39,04,40,230,46*77"
            };
        }
    }
}
