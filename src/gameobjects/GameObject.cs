using mike_and_conquer.gameworld;

using Point = Microsoft.Xna.Framework.Point;



namespace mike_and_conquer.gameobjects
{ 


    public abstract class GameObject
    {

        protected GameWorldLocation gameWorldLocation;

        public GameWorldLocation GameWorldLocation
        {
            get { return gameWorldLocation; }
        }


        protected int health;

        public int Health
        {
            get { return health;}
            set { health = value; }
        }


        protected int maxHealth;

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        protected UnitSize unitSize;

        public UnitSize UnitSize
        {
            get { return unitSize; }
        }



        // TODO:  Make this calculated rather than hard coded
        // Once I have gathered more data for more units
        // and can figure out the algorithm
        protected Point selectionCursorOffset;
        public Point SelectionCursorOffset
        {
            get { return selectionCursorOffset; }
        }


    }


}
