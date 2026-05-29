namespace Greenhouse.Core.Setup;

public sealed record GeneralConfigurationRequest(
    string GreenhouseName,
    string GreenhouseLocation,
    string? Description);
