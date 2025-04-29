using System;
using JetBrains.Application.Environment;
using JetBrains.Application.Parts;
using JetBrains.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.ProjectsHost.Impl;
using JetBrains.ProjectModel.ProjectsHost.SolutionHost;
using JetBrains.Util;
using ReSharperPlugin.RiderTestPlugin;

namespace ReSharperPlugin.RiderTestPlugin.Listeners;

[SolutionComponent(InstantiationEx.LegacyDefault)]
public class SolutionSyncListener : SolutionHostSyncListener
{
    private readonly ILogger _logger;
    public Action OnAfterSolutionUpdate { get; set; }

    public SolutionSyncListener(ILogger logger)
    {
        _logger = logger;
        logger.Log(LoggingLevel.INFO, "Solution sync listener created");
    }

    public override void AfterUpdateSolution(SolutionStructureChange change)
    {
        _logger.Log(LoggingLevel.INFO, $"{nameof(AfterUpdateSolution)} triggered");
        OnAfterSolutionUpdate?.Invoke();
    }

    public override void AfterRemoveProject(ProjectHostChange change)
    {
        _logger.Log(LoggingLevel.INFO, $"{nameof(AfterRemoveProject)} triggered");
        OnAfterSolutionUpdate?.Invoke();
    }

    public override void AfterUpdateProjects(ProjectStructureChange change)
    {
        _logger.Log(LoggingLevel.INFO, $"{nameof(AfterUpdateProjects)} triggered");
        OnAfterSolutionUpdate?.Invoke();
    }

    public override void AfterUpdateProject(ProjectHostChange change)
    {
        _logger.Log(LoggingLevel.INFO, $"{nameof(AfterUpdateProject)} triggered");
        OnAfterSolutionUpdate?.Invoke();
    }
}
