namespace Labo.ServiceModel.MessageInspector
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    [DataContract]
    public sealed class ServiceRequestInfo
    {
        [DataMember]
        public string RequestId { get; set; }

        [DataMember]
        public string SessionId { get; set; }
    }
}