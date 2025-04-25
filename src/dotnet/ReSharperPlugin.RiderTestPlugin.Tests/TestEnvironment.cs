using System.Threading;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.TestFramework;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: Apartment(ApartmentState.STA)]

namespace ReSharperPlugin.RiderTestPlugin.Tests
{
    [ZoneDefinition]
    public class RiderTestPluginTestEnvironmentZone : ITestsEnvZone, IRequire<PsiFeatureTestZone>, IRequire<IRiderTestPluginZone> { }

    [ZoneMarker]
    public class ZoneMarker : IRequire<ICodeEditingZone>, IRequire<ILanguageCSharpZone>, IRequire<RiderTestPluginTestEnvironmentZone> { }

    [SetUpFixture]
    public class RiderTestPluginTestsAssembly : ExtensionTestEnvironmentAssembly<RiderTestPluginTestEnvironmentZone> { }
}
