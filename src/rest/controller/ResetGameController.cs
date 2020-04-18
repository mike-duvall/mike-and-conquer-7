using System.Web.Http;
using mike_and_conquer.main;
using mike_and_conquer.rest.domain;

namespace mike_and_conquer.rest.controller
{
    public class ResetGameController : ApiController
    {

        public void Post([FromBody]RestResetOptions resetOptions)
        {
            GameWorld.instance.ResetGameViaEvent(resetOptions.drawShroud);
        }


    }
}