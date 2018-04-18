using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gameevent 
{
    public class ResetGameGameEvent : AsyncGameEvent
    {

        protected override GameState ProcessImpl()
        {
            return MikeAndConqueryGame.instance.HandleReset();
        }

    }
}
