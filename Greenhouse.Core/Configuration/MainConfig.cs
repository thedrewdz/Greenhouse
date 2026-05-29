namespace Greenhouse.Core.Configuration;

public sealed record MainConfig
{
    public required string GreenhouseName { get; init; }

    public required string GreenhouseLocation { get; init; }

    public string? Description { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; }

    public DateTimeOffset UpdatedAtUtc { get; init; }
}
