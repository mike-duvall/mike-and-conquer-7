
using System;

namespace mike_and_conquer.gameworld
{
    public class GameHistoryEvent
    {

        private String eventType;
        private int unitId;
        private ulong wallClockTime;

        public String EventType
        {
            get { return eventType; }
        }

        public int UnitId
        {
            get { return unitId; }
        }

        public ulong WallClockTime
        {
            get { return wallClockTime; }
        }

        public GameHistoryEvent(String evenType, int unitId, ulong wallClockTime)
        {
            this.eventType = evenType;
            this.unitId = unitId;
            this.wallClockTime = wallClockTime;
        }

    }
}
