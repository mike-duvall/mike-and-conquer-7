using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;

namespace mike_and_conquer.gameworld
{
    class GDIPlayer
    {


        private PlayerController playerController;

        public List<Minigunner> gdiMinigunnerList;
        public List<Minigunner> GdiMinigunnerList
        {
            get { return gdiMinigunnerList; }
        }



        private GDIBarracks gdiBarracks;
        public GDIBarracks GDIBarracks
        {
            get { return gdiBarracks; }
        }

        private GDIConstructionYard gdiConstructionYard;
        public GDIConstructionYard GDIConstructionYard
        {
            get { return gdiConstructionYard; }
        }



        private MCV mcv;
        public MCV MCV
        {
            get { return mcv; }
            set { mcv = value; }
        }

        public GDIPlayer(PlayerController playerController)
        {
            gdiMinigunnerList = new List<Minigunner>();
            this.playerController = playerController;
        }

        public void HandleReset()
        {
            gdiMinigunnerList.Clear();
            mcv = null;
            gdiConstructionYard = null;
            gdiBarracks = null;

        }

        public Minigunner GetMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                if (nextMinigunner.id == id)
                {
                    foundMinigunner = nextMinigunner;
                }
            }

            return foundMinigunner;

        }

        public void DeslectAllUnits()
        {
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                nextMinigunner.selected = false;
            }

            if (mcv != null)
            {
                mcv.selected = false;
            }

        }

        public void Update(GameTime gameTime)
        {
            playerController.Update(gameTime);
            UpdateGDIMinigunners(gameTime);
            UpdateBarracks(gameTime);
            UpdateConstructionYard(gameTime);



            if (mcv != null)
            {
                mcv.Update(gameTime);
            }

        }


        private void UpdateGDIMinigunners(GameTime gameTime)
        {
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                if (nextMinigunner.health > 0)
                {
                    nextMinigunner.Update(gameTime);
                }
            }
        }


        private void UpdateBarracks(GameTime gameTime)
        {
            if (gdiBarracks != null)
            {
                gdiBarracks.Update(gameTime);
            }
        }


        private void UpdateConstructionYard(GameTime gameTime)
        {
            if (gdiConstructionYard != null)
            {
                gdiConstructionYard.Update(gameTime);
            }
        }


        public void AddMinigunner(Minigunner newMinigunner)
        {
            gdiMinigunnerList.Add(newMinigunner);

        }

        public MCV AddMCV(Point positionInWorldCoordinates)
        {
            mcv = new MCV(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
            return mcv;
        }

        public GDIBarracks AddGDIBarracks(MapTileLocation mapTileLocation)
        {
            // TODO Might want to check if one already exists and throw error if so
            gdiBarracks = new GDIBarracks(mapTileLocation);
            return gdiBarracks;
        }

        public GDIConstructionYard AddGDIConstructionYard(Point positionInWorldCoordinates)
        {
            // TODO Might want to check if one already exists and throw error if so
            gdiConstructionYard = new GDIConstructionYard(positionInWorldCoordinates.X, positionInWorldCoordinates.Y);
            return gdiConstructionYard;
        }

        public bool IsPointOverMCV(Point pointInWorldCoordinates)
        {

            if (this.mcv != null)
            {
                if (mcv.ContainsPoint(pointInWorldCoordinates.X, pointInWorldCoordinates.Y))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAMinigunnerSelected()
        {
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                if (nextMinigunner.selected)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAnMCVSelected()
        {
            if (mcv != null)
            {
                return mcv.selected;
            }

            return false;
        }


        public void RemoveMCV()
        {
            mcv = null;
        }
    }
}
