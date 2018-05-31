
using Minigunner = mike_and_conquer.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;
using System.Collections.Generic;
using MikeAndConqueryGame = mike_and_conquer.MikeAndConqueryGame;

namespace mike_and_conquer_6.mike_and_conquer.aicontroller
{
    class MinigunnerAIController
    {
        private Minigunner myMinigunner;
        private Minigunner currentAttackTarget;

        private bool enemyStateIsSleeping;

        private static readonly int ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE = 1000;
        private int enemySleepCountdownTimer;


        public MinigunnerAIController(Minigunner minigunner)
        {
            this.myMinigunner = minigunner;
            this.enemyStateIsSleeping = true;
        }


        private Minigunner FindFirstNonDeadGdiMinigunner()
        {
            List<Minigunner> gdiMinigunners = (MikeAndConqueryGame.instance.gdiMinigunnerList);

            foreach (Minigunner nextMinigunner in gdiMinigunners)
            {
                if (nextMinigunner.health > 0)
                {
                    return nextMinigunner;
                }
            }

            return null;

        }


        public void HandleEnemyUpdate(GameTime gameTime)
        {
            if (!enemyStateIsSleeping)
            {


                Pickup here 

                Consider if we should just determine if current state is not attackking and if so make it attack

                    Consider if it should recalc attack target each time in case an enemey moves closer 

                if (currentAttackTarget != null && currentAttackTarget.health <= 0)
                {
                    currentAttackTarget = FindFirstNonDeadGdiMinigunner();

                    if (currentAttackTarget == null)
                    {
                        enemyStateIsSleeping = true;
                        enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;
                        return;
                    }

                }

                //if (myMinigunner.IsInAttackRange())
                //{
                //    this.state = State.ATTACKING;
                //    currentAttackTarget.ReduceHealth(10);
                //}
                //else
                //{
                //    this.state = State.MOVING;
                //    SetDestination((int)currentAttackTarget.position.X, (int)currentAttackTarget.position.Y);
                //    MoveTowardsDestination(gameTime);
                //}

            }
            else
            {
                this.state = State.IDLE;
                enemySleepCountdownTimer--;
                if (enemySleepCountdownTimer <= 0)
                {
                    enemyStateIsSleeping = false;
                    currentAttackTarget = FindFirstNonDeadGdiMinigunner();
                    if (currentAttackTarget == null)
                    {
                        enemyStateIsSleeping = true;
                        enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;
                    }

                }

            }

        }




    }
}
