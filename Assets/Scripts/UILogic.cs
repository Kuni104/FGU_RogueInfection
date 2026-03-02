using System;
using UnityEngine;

public class UILogic : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    private void Awake()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }

    public void OnCreateAccountButtonClicked()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
}
