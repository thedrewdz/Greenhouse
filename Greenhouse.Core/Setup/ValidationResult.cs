namespace Greenhouse.Core.Setup;

public sealed record ValidationResult(IReadOnlyList<string> Errors)
{
    public bool IsValid => Errors.Count == 0;

    public static ValidationResult Success { get; } = new(Array.Empty<string>());

    public static ValidationResult Failure(params string[] errors) => new(errors);

    public static ValidationResult Failure(IEnumerable<string> errors) => new(errors.ToArray());
}
