using System.Diagnostics;
using Greenhouse.Core.Onboarding;
using Greenhouse.Core.Onboarding.Abstractions;

namespace Greenhouse.UI.Infrastructure;

public sealed class BlueZEdgeUnitDiscoveryService(
    ILogger<BlueZEdgeUnitDiscoveryService> logger)
    : IEdgeUnitDiscoveryService
{
    public const string EdgeUnitNamePrefix = "GH-Edge-";

    public async Task<EdgeUnitScanResult> ScanAsync(
        TimeSpan duration,
        CancellationToken cancellationToken = default)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "bluetoothctl",
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        try
        {
            process.Start();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unable to start bluetoothctl for Edge Unit discovery.");
            return EdgeUnitScanResult.Failure("Bluetooth scanning is unavailable on this unit.");
        }

        try
        {
            var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

            await process.StandardInput.WriteLineAsync("scan on");
            await process.StandardInput.FlushAsync(cancellationToken);
            await Task.Delay(duration, cancellationToken);
            await process.StandardInput.WriteLineAsync("scan off");
            await process.StandardInput.WriteLineAsync("quit");
            await process.StandardInput.FlushAsync(cancellationToken);

            await process.WaitForExitAsync(cancellationToken);

            var output = await outputTask;
            var error = await errorTask;
            if (process.ExitCode != 0)
            {
                logger.LogWarning("bluetoothctl exited with code {ExitCode}: {Error}", process.ExitCode, error);
                return EdgeUnitScanResult.Failure("Bluetooth scan failed.");
            }

            return EdgeUnitScanResult.Success(ParseBluetoothctlOutput(output));
        }
        catch (OperationCanceledException)
        {
            StopProcess(process);
            throw;
        }
        catch (Exception ex)
        {
            StopProcess(process);
            logger.LogWarning(ex, "Bluetooth scan failed.");
            return EdgeUnitScanResult.Failure("Bluetooth scan failed.");
        }
    }

    public static IReadOnlyList<DiscoveredEdgeUnit> ParseBluetoothctlOutput(string output)
    {
        var devices = new Dictionary<string, MutableDevice>(StringComparer.OrdinalIgnoreCase);

        foreach (var rawLine in output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var line = rawLine.Trim();
            var deviceIndex = line.IndexOf("Device ", StringComparison.Ordinal);
            if (deviceIndex < 0)
            {
                continue;
            }

            var deviceText = line[(deviceIndex + "Device ".Length)..];
            var spaceIndex = deviceText.IndexOf(' ');
            if (spaceIndex <= 0)
            {
                continue;
            }

            var address = deviceText[..spaceIndex];
            var remainder = deviceText[(spaceIndex + 1)..].Trim();

            if (!devices.TryGetValue(address, out var device))
            {
                device = new MutableDevice(address);
                devices[address] = device;
            }

            if (remainder.StartsWith("Name:", StringComparison.OrdinalIgnoreCase)
                || remainder.StartsWith("Alias:", StringComparison.OrdinalIgnoreCase))
            {
                device.Name = remainder[(remainder.IndexOf(':') + 1)..].Trim();
            }
            else if (remainder.StartsWith("RSSI:", StringComparison.OrdinalIgnoreCase)
                     && int.TryParse(remainder[(remainder.IndexOf(':') + 1)..].Trim(), out var rssi))
            {
                device.Rssi = rssi;
            }
            else if (remainder.StartsWith(EdgeUnitNamePrefix, StringComparison.OrdinalIgnoreCase))
            {
                device.Name = remainder;
            }
        }

        return devices.Values
            .Where(device => device.Name.StartsWith(EdgeUnitNamePrefix, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(device => device.Rssi ?? int.MinValue)
            .Select(device => new DiscoveredEdgeUnit(device.Address, device.Name, device.Rssi))
            .ToArray();
    }

    private static void StopProcess(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch
        {
            // Process shutdown is best-effort during scan cancellation.
        }
    }

    private sealed class MutableDevice(string address)
    {
        public string Address { get; } = address;
        public string Name { get; set; } = string.Empty;
        public int? Rssi { get; set; }
    }
}
