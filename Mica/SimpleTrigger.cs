namespace Mica
{
    public class SimpleTrigger
    {
        DateTime LastTriggered { get; set; } = DateTime.MinValue;
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);
        public Type JobType { get; set; } 

        public DateTime GetTriggerMoment() => LastTriggered + Interval;
        public void Triggered() => LastTriggered = DateTime.Now;

    }
}
