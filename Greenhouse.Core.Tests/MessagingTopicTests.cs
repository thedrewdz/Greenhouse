using Greenhouse.Core.Messaging;

namespace Greenhouse.Core.Tests;

public sealed class MessagingTopicTests
{
    [Fact]
    public void ReadCommand_ReturnsDeviceSpecificTopic()
    {
        Assert.Equal("ghcmd/rd-1ADD5912AF61", MessagingTopics.ReadCommand("1ADD5912AF61"));
    }

    [Fact]
    public void WriteCommand_ReturnsDeviceSpecificTopic()
    {
        Assert.Equal("ghcmd/wr-F11234AABC1A", MessagingTopics.WriteCommand("F11234AABC1A"));
    }
}
