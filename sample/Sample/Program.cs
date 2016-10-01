using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Metrics;
using Metrics.Serilog;
using Serilog;
using Serilog.Events;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace Sample
{
    internal static class Program
    {
        private static Counter _totalRequestsCounter;
        private static Meter _requestsPerSecondMeter;
        private static Histogram _responseSizeHistogram;
        private static Timer _responseHandlerTimer;

        private static void Main()
        {
            const string seqServerUrl = "http://localhost:5341";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .WriteTo.Seq(seqServerUrl)
                .Enrich.FromLogContext()
                .CreateLogger();

            Console.WriteLine($"Writing Serilog events to Seq on {seqServerUrl}");

            Metric.Config
                .WithHttpEndpoint("http://localhost:1234/")
                .WithReporting(reports => reports
                    .WithSerilog(TimeSpan.FromSeconds(5), LogEventLevel.Information, levels => levels
                        .CountersAt(LogEventLevel.Information)
                        .GaugesAt(LogEventLevel.Information)
                        .HealthStatusAt(LogEventLevel.Information)
                        .HistogramsAt(LogEventLevel.Information)
                        .MetersAt(LogEventLevel.Information)
                        .TimersAt(LogEventLevel.Information)));

            var tags = new MetricTags(Environment.MachineName, Process.GetCurrentProcess().ProcessName);

            _totalRequestsCounter = Metric.Counter("TotalRequestsCounter", Unit.Requests, tags);
            _requestsPerSecondMeter = Metric.Meter("RequestsPerSecondMeter", Unit.Requests, tags: tags);
            _responseSizeHistogram = Metric.Histogram("ResponseSizeHistogram", Unit.Bytes, tags: tags);
            _responseHandlerTimer = Metric.Timer("ResponseHandlerTimer", Unit.Results, tags: tags);

            Metric.Gauge("WorkingSetGauge", () => Environment.WorkingSet, Unit.Bytes, tags);

            var proxyServer = new ProxyServer();
            var proxyEndpoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true);

            proxyServer.BeforeRequest += OnRequest;
            proxyServer.BeforeResponse += OnResponse;
            proxyServer.AddEndPoint(proxyEndpoint);
            proxyServer.Start();

            foreach (var endPoint in proxyServer.ProxyEndPoints)
                Console.WriteLine($"Proxy listening on IP {endPoint.IpAddress} and port {endPoint.Port}");

            proxyServer.SetAsSystemHttpProxy(proxyEndpoint);
            proxyServer.SetAsSystemHttpsProxy(proxyEndpoint);

            Console.Read();

            proxyServer.BeforeRequest -= OnRequest;
            proxyServer.BeforeResponse -= OnResponse;
            proxyServer.Stop();

            Log.CloseAndFlush();
        }

        private static Task OnRequest(object sender, SessionEventArgs e)
        {
            var method = e.WebSession.Request.Method;

            _totalRequestsCounter.Increment(method);

            _requestsPerSecondMeter.Mark(method);

            return Task.FromResult(0);
        }

        private static async Task OnResponse(object sender, SessionEventArgs e)
        {
            var host = e.WebSession.Request.RequestUri.Host;

            using (_responseHandlerTimer.NewContext(host))
            {
                if (e.WebSession.Response.ContentType != null)
                {
                    var bodyBytes = await e.GetResponseBody();
                    _responseSizeHistogram.Update(bodyBytes.Length, host);
                }
            }
        }
    }
}
