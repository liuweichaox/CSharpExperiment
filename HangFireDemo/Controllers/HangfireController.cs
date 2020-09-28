using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace HangFireDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly IBackgroundJobClient _jobClient;
        private readonly ILogger<HangfireController> _logger;
        private readonly HttpRequestTest _httpRequestTest;

        public HangfireController(IBackgroundJobClient jobClient,
            ILogger<HangfireController> logger,
            HttpRequestTest httpRequestTest)
        {
            _jobClient = jobClient;
            _logger = logger;
            _httpRequestTest = httpRequestTest;
        }

        [HttpGet("unico")]
        public string Enqueue()
        {
            var id = _jobClient.Enqueue(
                () => Console.WriteLine("Job de enfileiramento único executado"));
            _logger.LogInformation($"Job único enfileirado com ID: {id}");
            return id;
        }

        [HttpGet("delay")]
        public string EnqueueDelay()
        {
            var id = _jobClient.Schedule(
                () => Console.WriteLine("Job de enfileiramento com delay de 10s executado"),
                TimeSpan.FromSeconds(10));
            _logger.LogInformation($"Job com delay de 10s enfileirado com ID: {id}");
            return id;
        }

        [HttpGet("recorrente")]
        public string EnqueueRecurring()
        {
            RecurringJob.AddOrUpdate("my-recurring-job",
                () => Console.WriteLine($"Job recorrente executado as {DateTime.Now.ToShortTimeString()}"),
                Cron.Minutely);
            _logger.LogInformation($"Job Job recorrente enfileirado com ID: my-recurring-job");
            return "my-recurring-job";
        }

        [HttpGet("retry")]
        public string EnqueueRetry()
        {

            var id = _jobClient.Schedule(
             () => _httpRequestTest.ExecuteHttpRequest(),
             TimeSpan.FromSeconds(10));
            _logger.LogInformation($"Job único enfileirado com ID: {id}");
            return id;
        }
    }
}
