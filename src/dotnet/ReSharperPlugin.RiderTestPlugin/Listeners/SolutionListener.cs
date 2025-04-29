using System;
using JetBrains.Application.Parts;
using JetBrains.Diagnostics;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Tasks;
using JetBrains.Util;

namespace ReSharperPlugin.RiderTestPlugin.Listeners;

[SolutionComponent(InstantiationEx.LegacyDefault)]
public class SolutionListener
{
    private readonly ISolutionLoadTasksScheduler _solutionLoadTasksScheduler;
    private readonly SolutionSyncListener _solutionSyncListener;
    private readonly ILogger _logger;

    public Action OnAfterSolutionLoad { get; set; }
    public Action OnAfterSolutionUpdate { get; set; }

    public SolutionListener(
        ISolutionLoadTasksScheduler solutionLoadTasksScheduler, 
        SolutionSyncListener solutionSyncListener,
        ILogger logger)
    {
        _logger = logger;
        _solutionLoadTasksScheduler = solutionLoadTasksScheduler;
        _solutionSyncListener = solutionSyncListener;
    }

    public void Setup()
    {
        // Check if delegates empty?
        
        _solutionLoadTasksScheduler.EnqueueTask(new SolutionLoadTask(
            GetType(), SolutionLoadTaskKinds.AfterDone, () =>
            {
                _logger.Log(LoggingLevel.INFO, "Solution load task executed");
                
                OnAfterSolutionLoad?.Invoke();
                
                _solutionSyncListener.OnAfterSolutionUpdate += OnAfterSolutionUpdate;
            }));
    }
}