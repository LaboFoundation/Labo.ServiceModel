namespace Labo.ServiceModel.Client
{
    public sealed class IncomingMessageHeaderInfo
    {
        public string Name { get; private set; }

        public string Namespace { get; private set; }

        public object Value { get; private set; }

        public IncomingMessageHeaderInfo(string name, string @namespace, object value)
        {
            Name = name;
            Value = value;
            Namespace = @namespace;
        }
    }
}