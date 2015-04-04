namespace Labo.ServiceModel.Behavior
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    public class IPFilterServiceBehavior : IDispatchMessageInspector, IServiceBehavior
    {
        private static readonly object s_HttpAccessDenied = new object();
        private static readonly object s_AccessDenied = new object();

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)"/> method.
        /// </returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // RemoteEndpointMessageProperty new in 3.5 allows us to get the remote endpoint address.
            RemoteEndpointMessageProperty remoteEndpoint = request.Properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            // The address is a string so we have to parse to get as a number
            IPAddress address = IPAddress.Parse(remoteEndpoint.Address);

            // If ip address is denied clear the request mesage so service method does not get execute
            if (!true)//IPFilterBusiness.IsAllowedIP(GetIPAddress(address)))
            {
                request = null;
                return (channel.LocalAddress.Uri.Scheme.Equals(Uri.UriSchemeHttp) ||
                    channel.LocalAddress.Uri.Scheme.Equals(Uri.UriSchemeHttps)) ?
                    s_HttpAccessDenied : s_AccessDenied;
            }

            return null;
        }

        private static string GetIPAddress(IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                IPAddress address = GetIPv4Address(ipAddress);
                if (address == null)
                {
                    throw new NotSupportedException("IPv4 supported only.  Address Family : " + ipAddress.AddressFamily.ToString());
                }
                ipAddress = address;
            }
            return ipAddress.ToString();
        }

        private static IPAddress GetIPv4Address(IPAddress address)
        {
            if (IPAddress.IPv6Loopback.Equals(address))
            {
                return new IPAddress(0x0100007F);
            }
            IPAddress[] addresses = Dns.GetHostAddresses(address.ToString());
            return addresses.FirstOrDefault(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)"/> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (correlationState == s_HttpAccessDenied)
            {
                HttpResponseMessageProperty responseProperty = new HttpResponseMessageProperty
                    {
                        StatusCode = (HttpStatusCode) 401
                    };
                reply.Properties["httpResponse"] = responseProperty;                
            }
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param>
        /// <param name="serviceHostBase">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }
        
        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                SynchronizedCollection<EndpointDispatcher> endpoints = channelDispatcher.Endpoints;
                for (int i = 0; i < endpoints.Count; i++)
                {
                    EndpointDispatcher endpointDispatcher = endpoints[i];
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(this);
                }
            }
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            
        }
    }
}
