namespace Greenhouse.UI.Tests;

public sealed class UiAssemblyTests
{
    [Fact]
    public void ProgramType_IsAvailable()
    {
        Assert.Equal("Greenhouse.UI.Program", typeof(UI.Program).FullName);
    }
}
