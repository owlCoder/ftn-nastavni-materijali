using EquipmentReservation.Mcp.Processes;
using EquipmentReservation.Mcp.Workspace;
using Microsoft.Extensions.DependencyInjection;

namespace EquipmentReservation.Mcp.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectWorkspace(
        this IServiceCollection services,
        ProjectRoot root) =>
        services
            .AddSingleton(root)
            .AddSingleton<ProjectPathPolicy>()
            .AddSingleton<IProjectFileReader, ProjectFileReader>()
            .AddSingleton<IProjectStructureProvider, ProjectStructureProvider>()
            .AddSingleton<IProjectCommandRunner, ProjectCommandRunner>();
}
