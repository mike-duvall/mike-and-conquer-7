
namespace mike_and_conquer.rest.domain
{

    public class RestMinigunner
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int screenX { get; set; }
        public int screenY { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }
        public bool aiIsOn { get; set; }
        public int destinationX { get; set; }
        public int destinationY { get; set; }

    }

}
