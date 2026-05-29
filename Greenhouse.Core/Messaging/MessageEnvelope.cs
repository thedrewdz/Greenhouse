namespace Greenhouse.Core.Messaging;

public sealed record MessageEnvelope(string Topic, string Payload);
