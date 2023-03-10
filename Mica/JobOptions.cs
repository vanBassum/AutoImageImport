namespace Mica
{
    public class JobOptions
    {
        public List<TriggerConfiguration> Triggers { get; } = new List<TriggerConfiguration>();

        public JobOptions AddTrigger(Action<TriggerOptions> action = null)
        {
            var configuration = new TriggerConfiguration
            {

            };
            if (action != null)
            {
                var triggerOptions = new TriggerOptions();
                action?.Invoke(triggerOptions);
                configuration.Options = triggerOptions;
            }
            Triggers.Add(configuration);
            return this;
        }
    }
}
