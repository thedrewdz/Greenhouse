namespace Greenhouse.Core.Setup;

public sealed record NetworkConnectionResult(bool Succeeded, string? ErrorMessage = null)
{
    public static NetworkConnectionResult Success() => new(true);

    public static NetworkConnectionResult Failure(string errorMessage) => new(false, errorMessage);
}
