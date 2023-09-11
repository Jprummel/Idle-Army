using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroShopUI : MonoBehaviour
{
    [SerializeField] private HeroManifest m_HeroManifest;
    [SerializeField] private HeroBuyButton m_HeroBuyButtonPrefab;
    [SerializeField] private Transform m_ButtonSpawnParent;
    private int m_HighestUnlockedHeroIndex = -1;

    private void Awake()
    {
        GameEvents.OnHerosChanged += GetHighestUnlockedIndex;
    }

    private void OnDestroy()
    {
        GameEvents.OnHerosChanged -= GetHighestUnlockedIndex;
    }

    private void GetHighestUnlockedIndex()
    {
        int highestUnlockedHeroIndex = 0;
        for (int i = 0; i < m_HeroManifest.AllHeroes.Count; i++)
        {
            if (GameManager.Instance.HeroManager.Heros.ContainsKey(m_HeroManifest.AllHeroes[i]))
            {
                if (GameManager.Instance.HeroManager.Heros[m_HeroManifest.AllHeroes[i]] > 0)
                {
                    highestUnlockedHeroIndex = i;
                }
            }
        }

        if (highestUnlockedHeroIndex > m_HighestUnlockedHeroIndex)
        {
            m_HighestUnlockedHeroIndex = highestUnlockedHeroIndex;
            if (m_HighestUnlockedHeroIndex <= m_HeroManifest.AllHeroes.Count - 1)
            {
                HeroBuyButton newBuyButton = Instantiate(m_HeroBuyButtonPrefab, m_ButtonSpawnParent);
                newBuyButton.Spawn(m_HeroManifest.AllHeroes[m_HighestUnlockedHeroIndex + 1]);
            }
        }
    }
}
