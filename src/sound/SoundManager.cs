

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using mike_and_conquer.main;

namespace mike_and_conquer.sound
{
    public class SoundManager
    {

        private SoundEffect unitAwaitingOrders;
        private SoundEffect unitAffirmitive1;
        private SoundEffect evaBuilding;
        private Song song;

        public void LoadContent(ContentManager Content)
        {
            unitAwaitingOrders = Content.Load<SoundEffect>("Sounds/TDC_SFX_UNT_AWAIT1.V00_EN-US");
            unitAffirmitive1 = Content.Load<SoundEffect>("Sounds/TDC_SFX_UNT_AFFIRM1.V00_EN-US");
            evaBuilding = Content.Load<SoundEffect>("Sounds/TDC_SFX_EVA_BLDGING1_EN-US");
            song = Content.Load<Song>("Music/C&C Tiberian Dawn OST - Act on Instinct");
        }


        public void PlaySong()
        {
            if (GameOptions.PLAY_MUSIC)
            {
                MediaPlayer.Play(song);
            }
        }

        public void PlayUnitAwaitingOrders()
        {
            unitAwaitingOrders.Play();
        }

        public void PlayUnitAffirmative1()
        {
            unitAffirmitive1.Play();
        }

        public void PlayEVABuilding()
        {
            evaBuilding.Play();
        }


    }
}
