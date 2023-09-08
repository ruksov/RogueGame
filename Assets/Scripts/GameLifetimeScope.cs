using Rogue.Dungeon;
using Rogue.Resources;
using Rogue.Save;
using Rogue.Settings;
using VContainer;
using VContainer.Unity;

namespace Rogue
{
  public class GameLifetimeScope : LifetimeScope
  {
    public GameSettingsSO Settings;
    public GameResourcesSO Resources;
    public GameSavesSO Saves;
    
    protected override void Configure(IContainerBuilder builder)
    {
      builder.RegisterInstance(Settings);
      builder.RegisterInstance(Resources);
      builder.RegisterInstance(Saves);
      
      builder.Register<DungeonBuilder>(Lifetime.Singleton);
      
      builder.RegisterEntryPoint<GameManager.GameManager>();
    }
  }
}