using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    [SerializeField] private Button m_button;

    private void Awake()
    {
        m_button.onClick.AddListener(OnBuy); 
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnBuy);
    }

    public virtual void OnBuy()
    {

    }

    public virtual void RefreshUI()
    {

    }

    public virtual void OnValidate()
    {
        if(m_button == null)
        {
            m_button = GetComponent<Button>();
        }
    }
}
