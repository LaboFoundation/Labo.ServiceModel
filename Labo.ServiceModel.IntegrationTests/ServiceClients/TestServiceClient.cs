namespace Labo.ServiceModel.IntegrationTests.ServiceClients
{
    using Labo.ServiceModel.Client;
    using Labo.ServiceModel.IntegrationTests.Services;

    public sealed class TestServiceClient : BaseServiceClient<ITestService>, ITestService
    {
        public TestServiceClient(CustomOutgoingMessageHeaderCreatorCollection customOutgoingMessageHeaderCreators)
            : base(customOutgoingMessageHeaderCreators)
        {
        }

        public TestServiceClient(CustomOutgoingMessageHeaderCreatorCollection customOutgoingMessageHeaderCreators, string endPointConfigurationName)
            : base(customOutgoingMessageHeaderCreators, endPointConfigurationName)
        {
        }

        public int Sum(int x, int y)
        {
            return CallServiceOperation(s => s.Sum(x, y));
        }
    }

    public sealed class TestService1Client : BaseServiceClient<ITestService1>, ITestService1
    {
        public TestService1Client(CustomOutgoingMessageHeaderCreatorCollection customOutgoingMessageHeaderCreators)
            : base(customOutgoingMessageHeaderCreators)
        {
        }

        public TestService1Client(CustomOutgoingMessageHeaderCreatorCollection customOutgoingMessageHeaderCreators, string endPointConfigurationName)
            : base(customOutgoingMessageHeaderCreators, endPointConfigurationName)
        {
        }

        public int Product(int x, int y)
        {
            return CallServiceOperation(s => s.Product(x, y));
        }
    }
}
