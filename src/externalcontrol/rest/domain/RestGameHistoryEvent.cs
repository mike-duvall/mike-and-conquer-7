namespace mike_and_conquer.externalcontrol.rest.domain
{

    public class RestGameHistoryEvent
    {

        public string eventType { get; set; }
        public int unitId { get; set; }

        public ulong wallClockTime { get; set; }

    }

}
