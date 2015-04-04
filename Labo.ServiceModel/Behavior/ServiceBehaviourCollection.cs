namespace Labo.ServiceModel.Behavior
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel.Description;

    [Serializable]
    public sealed class ServiceBehaviourCollection : List<IServiceBehavior>
    {
        public ServiceBehaviourCollection()
            : base()
        {
        }

        public ServiceBehaviourCollection(IEnumerable<IServiceBehavior> collection)
            : base(collection)
        {
        }

        public ServiceBehaviourCollection(int capacity)
            : base(capacity)
        {
        }
    }
}