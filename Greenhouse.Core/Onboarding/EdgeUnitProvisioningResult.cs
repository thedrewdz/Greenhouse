namespace Greenhouse.Core.Onboarding;

public sealed record EdgeUnitProvisioningResult(
    bool Succeeded,
    int ErrorCode,
    string? ErrorMessage)
{
    public static EdgeUnitProvisioningResult Success()
    {
        return new EdgeUnitProvisioningResult(true, 0, null);
    }

    public static EdgeUnitProvisioningResult Failure(int errorCode, string errorMessage)
    {
        return new EdgeUnitProvisioningResult(false, errorCode, errorMessage);
    }
}
