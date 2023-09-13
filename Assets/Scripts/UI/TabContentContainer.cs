using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabContentContainer : MonoBehaviour
{
    [SerializeField] private GameObject m_Content;

    public GameObject Content => m_Content;

    public virtual void Show()
    {
        m_Content.SetActive(true);
    }

    public virtual void Hide() 
    {
        m_Content.SetActive(false);
    }
}
