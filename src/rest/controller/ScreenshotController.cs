using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using mike_and_conquer.main;

namespace mike_and_conquer.rest.controller
{

    public class ScreenshotController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Generate()
        {
            MemoryStream stream = GameWorld.instance.GetScreenshotViaEvent();

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(stream.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "screenshot.png"
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }


    }


}