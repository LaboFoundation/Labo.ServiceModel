namespace Labo.ServiceModel
{
    using System;
    using System.ServiceModel.Channels;

    public sealed class CustomOutgoingMessageHeaderCreator : ICustomOutgoingMessageHeaderCreator
    {
        private readonly string m_HeaderNameSpace;

        private readonly string m_HeaderName;

        private readonly Func<object> m_MessageObjectCreator;

        public CustomOutgoingMessageHeaderCreator(string headerNameSpace, string headerName, Func<object> messageObjectCreator = null)
        {
            m_HeaderNameSpace = headerNameSpace;
            m_HeaderName = headerName;
            m_MessageObjectCreator = messageObjectCreator;
        }

        public MessageHeader CreateMessageHeader()
        {
            return CreateMessageHeader(m_MessageObjectCreator());
        }

        public MessageHeader CreateMessageHeader(object value)
        {
            return MessageHeader.CreateHeader(m_HeaderNameSpace, m_HeaderName, value);
        }
    }
}