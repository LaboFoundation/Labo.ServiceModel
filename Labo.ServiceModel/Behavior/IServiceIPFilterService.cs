namespace Labo.ServiceModel.Behavior
{
    public interface IServiceIPFilterService
    {
        bool IsAllowedIP(string serviceAddress, string actionName, string ipAddress);
    }
}
