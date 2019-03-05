using System;

using ManualResetEvent = System.Threading.ManualResetEvent;

namespace mike_and_conquer.gameevent
{
    public abstract class AsyncGameEvent
    {

        protected abstract GameState ProcessImpl();
        protected Object result;

        private ManualResetEvent condition;


        public AsyncGameEvent()
        {
            this.result = null;
            bool signaled = false;
            this.condition = new ManualResetEvent(signaled);
        }

        public GameState Process()
        {
            GameState newGameState = ProcessImpl();
            condition.Set();
            return newGameState;
        }


        // TODO Consider making an abstract SetResult() method
        // to force people to make a conscious decision on 
        // setting a result
        public Object GetResult()
        {
            condition.WaitOne();
            return result;
        }


    }
}
