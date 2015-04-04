namespace Labo.ServiceModel
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class CustomOutgoingMessageHeaderCreatorCollection : List<ICustomOutgoingMessageHeaderCreator>
    {
        public CustomOutgoingMessageHeaderCreatorCollection()
            : base()
        {
        }

        public CustomOutgoingMessageHeaderCreatorCollection(IEnumerable<ICustomOutgoingMessageHeaderCreator> collection)
            : base(collection)
        {
        }

        public CustomOutgoingMessageHeaderCreatorCollection(int capacity)
            : base(capacity)
        {
        }
    }
}