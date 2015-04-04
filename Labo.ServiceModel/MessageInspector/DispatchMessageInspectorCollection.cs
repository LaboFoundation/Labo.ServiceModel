namespace Labo.ServiceModel.MessageInspector
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel.Dispatcher;

    [Serializable]
    public sealed class DispatchMessageInspectorCollection : List<IDispatchMessageInspector>
    {
        public DispatchMessageInspectorCollection()
            : base()
        {
        }

        public DispatchMessageInspectorCollection(IEnumerable<IDispatchMessageInspector> collection)
            : base(collection)
        {
        }

        public DispatchMessageInspectorCollection(int capacity)
            : base(capacity)
        {
        }
    }
}