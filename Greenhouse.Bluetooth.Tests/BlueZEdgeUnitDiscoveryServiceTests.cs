using Greenhouse.Bluetooth;

namespace Greenhouse.Bluetooth.Tests;

public sealed class BlueZEdgeUnitDiscoveryServiceTests
{
    [Fact]
    public void ParseBluetoothctlOutput_ReturnsOnlyAdvertisedEdgeUnits()
    {
        const string output = """
                              [NEW] Device AA:BB:CC:DD:EE:01 GH-Edge-1ADD5912AF61
                              [CHG] Device AA:BB:CC:DD:EE:01 RSSI: -47
                              [NEW] Device AA:BB:CC:DD:EE:02 Kitchen Speaker
                              [CHG] Device AA:BB:CC:DD:EE:03 Name: GH-Edge-F11234AABC1A
                              [CHG] Device AA:BB:CC:DD:EE:03 RSSI: -66
                              """;

        var edgeUnits = BlueZEdgeUnitDiscoveryService.ParseBluetoothctlOutput(output);

        Assert.Equal(2, edgeUnits.Count);
        Assert.Equal("GH-Edge-1ADD5912AF61", edgeUnits[0].Name);
        Assert.Equal("AA:BB:CC:DD:EE:01", edgeUnits[0].Address);
        Assert.Equal(-47, edgeUnits[0].Rssi);
        Assert.Equal("GH-Edge-F11234AABC1A", edgeUnits[1].Name);
        Assert.Equal("AA:BB:CC:DD:EE:03", edgeUnits[1].Address);
        Assert.Equal(-66, edgeUnits[1].Rssi);
    }
}
