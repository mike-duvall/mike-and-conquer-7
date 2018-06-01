
using Minigunner = mike_and_conquer.Minigunner;
using GameTime = Microsoft.Xna.Framework.GameTime;
using System.Collections.Generic;
using MikeAndConqueryGame = mike_and_conquer.MikeAndConqueryGame;

namespace mike_and_conquer.aicontroller
{
    public class MinigunnerAIController
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
            this.enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;

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


        public void Update(GameTime gameTime)
        {
            if (!enemyStateIsSleeping)
            {


                //Pickup here 

                //Consider if we should just determine if current state is not attackking and if so make it attack

                //    Consider if it should recalc attack target each time in case an enemey moves closer 

                myMinigunner.OrderToMoveToAndAttackEnemyUnit(FindFirstNonDeadGdiMinigunner());

                //if (myMinigunner.state == Minigunner.State.IDLE)
                //{
                //    myMinigunner.OrderToMoveToAndAttackEnemyUnit(FindFirstNonDeadGdiMinigunner());
                //}
                //else 


                //if (currentAttackTarget != null && currentAttackTarget.health <= 0)
                //{
                //    currentAttackTarget = FindFirstNonDeadGdiMinigunner();

                //    if (currentAttackTarget == null)
                //    {
                //        enemyStateIsSleeping = true;
                //        enemySleepCountdownTimer = ENEMY_SLEEP_COUNTDOWN_TIMER_INITIAL_VALUE;
                //        return;
                //    }

                //}

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
