namespace mike_and_conquer.gamesprite
{
    public class NodSecondaryShpFileColorMapper : ShpFileColorMapper
    {

        public override int MapColorIndex(int index)
        {

            if (index == 176)
                return 127;

            if (index == 177)
                return 126;

            if (index == 178)
                return 125;

            if (index == 179)
                return 124;

            if (index == 180)
                return 122;

            if (index == 181)
                return 46;

            if (index == 182)
                return 120;

            if (index == 183)
                return 47;

            if (index == 184)
                return 125;

            if (index == 185)
                return 124;

            if (index == 186)
                return 123;

            if (index == 187)
                return 122;

            if (index == 188)
                return 42;

            if (index == 189)
                return 121;

            if (index == 190)
                return 120;

            if (index == 191)
                return 120;

            return index;

        }
    }
}
