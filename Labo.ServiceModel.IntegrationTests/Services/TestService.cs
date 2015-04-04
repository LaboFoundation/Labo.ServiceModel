namespace Labo.ServiceModel.IntegrationTests.Services
{
    using System.ServiceModel;

    using Labo.ServiceModel.IntegrationTests.ServiceClients;

    public sealed class TestService : ITestService
    {
        public int Sum(int x, int y)
        {
            OperationContext.Current.IncomingMessageHeaders.ToString();

            TestService1Client testServiceClient = new TestService1Client(new CustomOutgoingMessageHeaderCreatorCollection(), "TestService1WsHttpBinding");

            return x + testServiceClient.Product(x, y);
        }
    }

    public sealed class TestService1 : ITestService1
    {
        public int Product(int x, int y)
        {
            return x * y;
        }
    }
}