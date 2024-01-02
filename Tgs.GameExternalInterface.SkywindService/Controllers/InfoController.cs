using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameExternalInterface.SkywindService.Core;

namespace Tgs.GameExternalInterface.SkywindService.Controllers;

[Route("[controller]")]
[ApiController]
public class InfoController(
    IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public async Task<object> GetAsync([FromServices] IWebHostEnvironment _hostingEnvironment)
    {
        var hostName = Dns.GetHostName();
        var hostEntry = await Dns.GetHostEntryAsync(hostName);
        var hostIp = Array.Find(hostEntry.AddressList,
            x => x.AddressFamily == AddressFamily.InterNetwork);
        var gameExternalInterfaceSettingsOptions = configuration.GetSection("GameExternalInterfaceSettings").Get<GameExternalInterfaceSettingsOptions>();

        return new
        {
            Environment.MachineName,
            HostName = hostName,
            HostIp = hostIp?.ToString() ?? string.Empty,
            Environment = _hostingEnvironment.EnvironmentName,
            OsVersion = $"{Environment.OSVersion}",
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
            ProcessCount = Environment.ProcessorCount,
            ReleaseNamespace = configuration.GetValue<string>("RELEASE_NAMESPACE"),
            PodIPAddress = configuration.GetValue<string>("POD_IP"),
            GameExternalInterfaceSettingsOptions = gameExternalInterfaceSettingsOptions,
        };
    }
}
