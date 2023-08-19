using Rogue.Settings;
using VContainer;
using VContainer.Unity;

namespace Rogue
{
  public class GameLifetimeScope : LifetimeScope
  {
    public GameSettingsData Settings;
    
    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterInstance(Settings);
      builder.RegisterEntryPoint<GameManager.GameManager>();
    }
  }
}