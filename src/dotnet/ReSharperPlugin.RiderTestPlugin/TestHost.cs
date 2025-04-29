using System.Linq;
using JetBrains.Application.Parts;
using JetBrains.Diagnostics;
using JetBrains.Lifetimes;
using JetBrains.Platform.RdFramework.Impl;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.NuGet.Configs;
using JetBrains.ReSharper.Feature.Services.Protocol;
using JetBrains.ReSharper.PsiGen.Util;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Rider.Plugins.TestPlugin.Rd;
using ReSharperPlugin.RiderTestPlugin.Listeners;
using ILogger = JetBrains.Util.ILogger;

namespace ReSharperPlugin.RiderTestPlugin;

[SolutionComponent(InstantiationEx.LegacyDefault)]
public class TestHost
{
    private readonly TestModel _protocolModel;
    private readonly ISolution _solution;
    private readonly ShellRdDispatcher _shellRdDispatcher;
    private readonly ILogger _logger;
    private readonly NuGetConfigManager _nugetConfigManager;

    public TestHost(
        ISolution solution,
        SolutionListener solutionListener,
        ShellRdDispatcher shellRdDispatcher,
        NuGetConfigManager nuGetConfigManager,
        ILogger logger,
        Lifetime lifetime)
    {
        _nugetConfigManager = nuGetConfigManager;
        _solution = solution;
        _logger = logger;
        _shellRdDispatcher = shellRdDispatcher;
        
        _protocolModel = _solution.GetProtocolSolution().GetTestModel();

        _nugetConfigManager.OnChangesInConfig.Advise(lifetime, UpdatePackageSources);
        UpdatePackageSources(_nugetConfigManager.EffectiveConfig.Value);

        solutionListener.OnAfterSolutionLoad += UpdateDefaultProjects;
        solutionListener.OnAfterSolutionUpdate += UpdateDefaultProjects;
        solutionListener.OnAfterSolutionUpdate += UpdateDefaultProjects;
        
        solutionListener.Setup();
    }

    private void UpdateDefaultProjects()
    {
        using var cookie = WriteLockCookie.Create();

        // TODO: Add additional logic
        var defaultProjects = _solution
            .GetAllProjects()
            .Where(p => (p.ProjectProperties.ProjectKind & ProjectKind.REGULAR_PROJECT) != 0)
            .Select(p => new ProjectInfo(p.Guid ,p.Name));
        
        _logger.Log(LoggingLevel.INFO, "Default projects: " + string.Join(", ", defaultProjects));
        
        _shellRdDispatcher.Queue(() =>
        {
            _protocolModel.Projects.Clear();
            _protocolModel.Projects.AddAll(defaultProjects);
            _logger.Log(LoggingLevel.INFO, "Protocol projects: " + string.Join(", ", _protocolModel.Projects));
        });
    }
    
    private void UpdatePackageSources(NuGetSmartConfig config)
    {
        var sources = config.Complete.EnabledFeeds
            .Select(f => new SourceInfo(f.FeedId, f.Name));
        
        _logger.Log(LoggingLevel.INFO, "Package sources: " + string.Join(", ", sources));
        
        _shellRdDispatcher.Queue(() =>
        {
            _protocolModel.Sources.Clear();
            _protocolModel.Sources.AddAll(sources);
            _logger.Log(LoggingLevel.INFO, "Protocol sources: " + string.Join(", ", _protocolModel.Sources));
        });
    }
}