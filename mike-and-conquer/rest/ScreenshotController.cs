using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gameview;
using SharpDX.MediaFoundation;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Point = Microsoft.Xna.Framework.Point;
using BadMinigunnerLocationException = mike_and_conquer.GameWorld.BadMinigunnerLocationException;

namespace mike_and_conquer.rest
{

    public class ScreenshotController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Generate()
        {
            var stream = new MemoryStream();

            Pickup here
            This basic screenshotting is working, but need to 
            update it so that it happens via Event
            as it sometimes intertwines with Drawing in the main 
            loop and gets an error

            Make it something like:
            MemoryStream stream = GameWorld.instance.GetScreenshotViaEvent()

            stream = MikeAndConquerGame.instance.SaveScreenshot(stream);

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