using Boolean = System.Boolean;

using System.Collections.Generic;

namespace mike_and_conquer.util
{
    public class AnimationSequence
    {

        private List<uint> frames;
        private int frameSwitchTimer;
        private int frameSwitchThreshold;
        private int currentAnimationFrameIndex;
        private Boolean animate;

        public AnimationSequence(int frameSwitchThreshold)
        {
            this.frameSwitchTimer = 0;
            this.frameSwitchThreshold = frameSwitchThreshold;
            this.frames = new List<uint>();
        }

        public void AddFrame(uint frame)
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


        public uint GetCurrentFrame()
        {
            return frames[currentAnimationFrameIndex];
        }





    }
}
