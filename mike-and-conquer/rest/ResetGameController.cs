using System.Collections.Generic;
using System.Web.Http;

namespace mike_and_conquer.rest
{
    public class ResetGameController : ApiController
    {

        public void Post()
        {
            GameWorld.instance.ResetGameViaEvent();
        }


    }
}