using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Manifest", menuName = "Idle Army/Manifests/Hero Manifest", order = 0)]
public class HeroManifest : BaseManifestData
{
    [SerializeField] private List<HeroData> m_AllHeroes = new List<HeroData>();

    public List<HeroData> AllHeroes => m_AllHeroes;

    public HeroData GetHero(Heros hero)
    {
        HeroData requestedHero = null;
        foreach (HeroData heroData in m_AllHeroes)
        {
            if(heroData.Hero == hero)
            {
                requestedHero = heroData;
            }
        }

        return requestedHero;
    }

    public override void GatherData()
    {
        base.GatherData();
        m_AllHeroes.Clear();
        foreach (var hero in AssetDatabaseUtils.FindAssetsByType<HeroData>())
        {
            m_AllHeroes.Add(hero);
        }
    }
}