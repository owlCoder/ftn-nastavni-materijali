using EquipmentReservation.Mcp.Composition;
using EquipmentReservation.Mcp.Workspace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var root = ProjectRootLocator.Find(Environment.CurrentDirectory);
if (root.FullPath != string.Empty)
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);
    builder.Services.AddProjectWorkspace(root);
    builder.Services.AddMcpServer().WithStdioServerTransport().WithToolsFromAssembly().WithResourcesFromAssembly();
    await builder.Build().RunAsync();
}
else
{
    Console.Error.WriteLine("Project root was not found.");
}
