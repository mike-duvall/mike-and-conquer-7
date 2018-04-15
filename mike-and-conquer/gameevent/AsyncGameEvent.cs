using System;

using AutoResetEvent = System.Threading.AutoResetEvent;

namespace mike_and_conquer.gameevent
{
    public abstract class AsyncGameEvent
    {

        public AsyncGameEvent()
        {
            this.result = null;
            bool signaled = false;
            this.conditon = new AutoResetEvent(signaled);
        }

        public GameState Process()
        {
            GameState newGameState = ProcessImpl();
            conditon.Set();
            return newGameState;
        }

        public Object GetResult()
        {
            conditon.WaitOne();
            return result;
        }



        protected abstract GameState ProcessImpl();
	    protected Object result;

        private AutoResetEvent conditon;


    }
}
