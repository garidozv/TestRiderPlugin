using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.Platform.RdFramework;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.NuGet;
using JetBrains.Rider.Model;

namespace ReSharperPlugin.RiderTestPlugin
{
    [ZoneDefinition]
    // [ZoneDefinitionConfigurableFeature("Title", "Description", IsInProductSection: false)]
    public interface IRiderTestPluginZone : IZone,
        IRequire<IProjectModelZone>,
        IRequire<IRdFrameworkZone>
    {
    }
}