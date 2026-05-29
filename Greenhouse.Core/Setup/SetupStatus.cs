namespace Greenhouse.Core.Setup;

public sealed record SetupStatus(bool IsSetupComplete, SetupStartStep StartStep);
