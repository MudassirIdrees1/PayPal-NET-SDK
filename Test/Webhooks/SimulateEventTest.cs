using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using BraintreeHttp;
using Xunit;
using PayPal.Test;
using System.Threading.Tasks;

namespace PayPal.Webhooks.Test
{
    [Collection("Webhooks")]
    public class SimulateEventTest
    {
        private static SimulateEvent buildRequestBody()
        {
            var jsonContent = new StringContent("{ \"url\": \"https://www.ebay.com/paypal_webhook\", \"event_type\": \"PAYMENT.AUTHORIZATION.CREATED\" }", Encoding.UTF8, "application/json");
            return (SimulateEvent) new JsonSerializer().Decode(jsonContent, typeof(SimulateEvent));
        }

        public static async Task<HttpResponse> simulateEvent() {
            SimulateEventRequest request = new SimulateEventRequest();
            request.RequestBody(buildRequestBody());

            return await TestHarness.client().Execute(request);
        }

        [Fact]
        public async void TestSimulateEventRequest()
        {
            HttpResponse response = await simulateEvent();
            Assert.Equal(202, (int) response.StatusCode);
            Assert.NotNull(response.Result<Event>());
            var simulatedEvent = response.Result<Event>();
            Assert.Equal("PAYMENT.AUTHORIZATION.CREATED", simulatedEvent.EventType);
        }
    }
}