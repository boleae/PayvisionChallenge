namespace Refactoring.FraudDetection.Interfaces
{
    public interface INormalizationService
    {
        string NormalizeEmail(string email);
        string NormalizeStreet(string street);
        string NormalizeState(string state);
    }
}
