namespace Labo.ServiceModel
{
    using System.ServiceModel.Channels;

    public interface ICustomOutgoingMessageHeaderCreator
    {
        MessageHeader CreateMessageHeader();

        MessageHeader CreateMessageHeader(object value);
    }
}