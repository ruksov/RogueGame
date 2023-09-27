using System;
using Rogue.Dungeon;
using Rogue.NodeGraph;
using Rogue.Save;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rogue.Hero
{
  public class HeroProvider
  {
    public Hero Hero;

    public Action HeroCreated;
    public Action HeroDestroyed;

    public bool IsHeroCreated => Hero != null;
    
    private readonly HeroFactory m_heroFactory;
    private readonly DungeonBuilder m_dungeonBuilder;
    private readonly GameSavesSO m_saves;

    public HeroProvider(HeroFactory heroFactory, DungeonBuilder dungeonBuilder, GameSavesSO saves)
    {
      m_heroFactory = heroFactory;
      m_dungeonBuilder = dungeonBuilder;
      m_saves = saves;
    }

    public void CreateHero()
    {
      Hero = m_heroFactory.Create(m_saves.PlayerSave.Hero, InitSpawnPosition());
      HeroCreated?.Invoke();
    }

    public void DestroyHero()
    {
      HeroDestroyed?.Invoke();
      Object.Destroy(Hero.gameObject);
      Hero = null;
    }

    private Vector3 InitSpawnPosition() => 
      m_dungeonBuilder.RoomOfType(ENodeType.Entrance).RandomWorldSpawnPosition();
  }
}