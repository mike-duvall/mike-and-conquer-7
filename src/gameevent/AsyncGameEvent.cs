using System;
using mike_and_conquer.gamestate;
using ManualResetEvent = System.Threading.ManualResetEvent;

namespace mike_and_conquer.gameevent
{
    public abstract class AsyncGameEvent
    {
        protected Object result;
        private ManualResetEvent condition;
        private Exception thrownException;

        public AsyncGameEvent()
        {
            this.result = null;
            bool signaled = false;
            this.condition = new ManualResetEvent(signaled);
            this.thrownException = null;
        }

        protected abstract GameState ProcessImpl();


        public GameState Process()
        {
            GameState newGameState = null;
            try
            {
                newGameState = ProcessImpl();
            }
            catch (Exception e)
            {
                thrownException = e;
            }

            condition.Set();
            return newGameState;
        }


        // TODO Consider making an abstract SetResult() method
        // to force people to make a conscious decision on 
        // setting a result
        public Object GetResult()
        {
            condition.WaitOne();
            if (thrownException != null)
            {
                throw thrownException;
            }
            else
            {
                return result;
            }
        }


    }
}
