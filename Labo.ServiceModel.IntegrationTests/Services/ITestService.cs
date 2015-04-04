namespace Labo.ServiceModel.IntegrationTests.Services
{
    using System.ServiceModel;

    [ServiceContract]
    public interface ITestService
    {
        [OperationContract]
        int Sum(int x, int y);
    }

    [ServiceContract]
    public interface ITestService1
    {
        [OperationContract]
        int Product(int x, int y);
    }
}
