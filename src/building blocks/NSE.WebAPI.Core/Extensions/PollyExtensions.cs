using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.WebAPI.Core.Extensions
{
    public static class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {
            var retryWaitPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                });

            return retryWaitPolicy;
        }
    }
}
