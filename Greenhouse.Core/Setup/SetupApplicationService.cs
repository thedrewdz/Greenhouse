using Greenhouse.Core.Configuration;
using Greenhouse.Core.Setup.Abstractions;

namespace Greenhouse.Core.Setup;

public sealed class SetupApplicationService(
    IMainConfigRepository mainConfigRepository,
    INetworkService networkService)
{
    public const int GreenhouseNameMaxLength = 80;
    public const int GreenhouseLocationMaxLength = 120;
    public const int DescriptionMaxLength = 500;

    public async Task<SetupStatus> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        var config = await mainConfigRepository.GetAsync(cancellationToken);
        if (config is not null)
        {
            return new SetupStatus(true, SetupStartStep.Complete);
        }

        var isConnected = await networkService.IsConnectedAsync(cancellationToken);

        return new SetupStatus(
            false,
            isConnected ? SetupStartStep.GeneralInformation : SetupStartStep.NetworkConnection);
    }

    public ValidationResult ValidateNetworkCredentials(NetworkCredentials credentials)
    {
        if (string.IsNullOrWhiteSpace(credentials.NetworkName))
        {
            return ValidationResult.Failure("Network name is required.");
        }

        return ValidationResult.Success;
    }

    public async Task<NetworkConnectionResult> ConnectNetworkAsync(
        NetworkCredentials credentials,
        CancellationToken cancellationToken = default)
    {
        var validation = ValidateNetworkCredentials(credentials);
        if (!validation.IsValid)
        {
            return NetworkConnectionResult.Failure(validation.Errors[0]);
        }

        var normalizedCredentials = credentials with
        {
            NetworkName = credentials.NetworkName.Trim()
        };

        return await networkService.ConnectAsync(normalizedCredentials, cancellationToken);
    }

    public ValidationResult ValidateGeneralConfiguration(GeneralConfigurationRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.GreenhouseName))
        {
            errors.Add("Greenhouse name is required.");
        }
        else if (request.GreenhouseName.Trim().Length > GreenhouseNameMaxLength)
        {
            errors.Add($"Greenhouse name must be {GreenhouseNameMaxLength} characters or fewer.");
        }

        if (string.IsNullOrWhiteSpace(request.GreenhouseLocation))
        {
            errors.Add("Greenhouse location is required.");
        }
        else if (request.GreenhouseLocation.Trim().Length > GreenhouseLocationMaxLength)
        {
            errors.Add($"Greenhouse location must be {GreenhouseLocationMaxLength} characters or fewer.");
        }

        if (!string.IsNullOrWhiteSpace(request.Description)
            && request.Description.Trim().Length > DescriptionMaxLength)
        {
            errors.Add($"Description must be {DescriptionMaxLength} characters or fewer.");
        }

        return errors.Count == 0
            ? ValidationResult.Success
            : ValidationResult.Failure(errors);
    }

    public async Task<ValidationResult> WriteGeneralConfigurationAsync(
        GeneralConfigurationRequest request,
        CancellationToken cancellationToken = default)
    {
        var validation = ValidateGeneralConfiguration(request);
        if (!validation.IsValid)
        {
            return validation;
        }

        var now = DateTimeOffset.UtcNow;
        var existing = await mainConfigRepository.GetAsync(cancellationToken);

        var config = new MainConfig
        {
            GreenhouseName = request.GreenhouseName.Trim(),
            GreenhouseLocation = request.GreenhouseLocation.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            CreatedAtUtc = existing?.CreatedAtUtc ?? now,
            UpdatedAtUtc = now
        };

        await mainConfigRepository.SaveAsync(config, cancellationToken);

        return ValidationResult.Success;
    }
}
