using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HangFireDemo
{
    public class HttpRequestTest
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HttpRequestTest> _logger;

        public HttpRequestTest(IHttpClientFactory clientFactory,
            ILogger<HttpRequestTest> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task ExecuteHttpRequest()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("http://demo2366024.mockable.io/test-hangfire");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("HTTP Request with Success!");
            }
            else
            {
                _logger.LogError("HTTP Request failed!");
                throw new HttpRequestException(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
