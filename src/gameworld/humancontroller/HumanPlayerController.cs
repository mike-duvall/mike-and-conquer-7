using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;



namespace mike_and_conquer.gameworld.humancontroller
{
    class HumanPlayerController : PlayerController
    {

        private HumanControllerState humanControllerState;

        public static HumanPlayerController instance;

        private MouseState oldMouseState;


        public MouseState MouseState
        {
            get { return oldMouseState; }
        }

        public HumanPlayerController()
        {
            instance = this;
            humanControllerState = new NeutralMapstate();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();
            humanControllerState = humanControllerState.Update(gameTime, newMouseState, oldMouseState);
            oldMouseState = newMouseState;
        }


        public override void Add(Minigunner minigunner, bool aiIsOn)
        {
            // Do nothing
            // TODO: This was added to AI controller could know about new minigunners
            // Reconsider how this is handled
        }


    }
}
