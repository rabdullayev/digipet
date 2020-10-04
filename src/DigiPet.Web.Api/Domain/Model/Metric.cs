
namespace DigiPet.Web.Api.Domain.Model
{
    public abstract class Metric
    {
        protected readonly object SyncObject = new object();
        public abstract MetricType Type { get; }
        protected Metric(float value, float decreaseRate)
        {
            Value = value;
            DecreaseRate = decreaseRate;
        }
        
        public float Value { get; protected set; }
        public float DecreaseRate { get; protected set; }

        public virtual void Decrease(float delta)
        {
            lock (SyncObject)
            {
                Value -= DecreaseRate * delta;
            }
        }

        public virtual void Boost(float amount)
        {
            lock (SyncObject)
            {
                Value += amount;
            }
        }
    }
}
