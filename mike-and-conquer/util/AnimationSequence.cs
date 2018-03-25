using Boolean = System.Boolean;

using System.Collections.Generic;

namespace mike_and_conquer.util
{
    public class AnimationSequence
    {

        private List<int> frames;
        private int frameSwitchTimer;
        private int frameSwitchThreshold;
        private int currentAnimationFrameIndex;
        private Boolean animate;

        public AnimationSequence(int frameSwitchThreshold)
        {
            this.frameSwitchTimer = 0;
            this.frameSwitchThreshold = frameSwitchThreshold;
            this.frames = new List<int>();
        }

        public void SetAnimate(bool newValue)
        {
            animate = newValue;
        }

        public void AddFrame(int frame)
        {
            frames.Add(frame);
        }

        public void Update()
        {
            if (!animate)
            {
                return;
            }

            if (frameSwitchTimer > frameSwitchThreshold)
            {
                frameSwitchTimer = 0;
                currentAnimationFrameIndex++;
                if (currentAnimationFrameIndex >= frames.Count - 1)
                {
                    currentAnimationFrameIndex = 0;
                }
            }
            else
            {
                frameSwitchTimer++;
            }

        }


        public int GetCurrentFrame()
        {
            return frames[currentAnimationFrameIndex];
        }





    }
}
