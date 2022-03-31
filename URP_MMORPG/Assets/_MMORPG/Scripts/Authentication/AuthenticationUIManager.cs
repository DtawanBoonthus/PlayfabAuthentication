using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class AuthenticationUIManager : MonoBehaviour
{
    [Header("Add Tap")] 
    [SerializeField] private GameObject logInTap;
    [SerializeField] private GameObject registerTap;

    [Header("Add Button")] 
    [SerializeField] private Button logInButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button buttonSwitchToLogInTap;
    [SerializeField] private Button buttonSwitchToRegisterTap;
    
    private void Start()
    {
        buttonSwitchToLogInTap.onClick.AddListener(OnClickButtonSwitchToLogInTap);
        buttonSwitchToRegisterTap.onClick.AddListener(OnClickButtonSwitchToRegisterTap);
    }

    private void OnClickButtonSwitchToLogInTap()
    {
        logInTap.SetActive(true);
        registerTap.SetActive(false);
    }
    
    private void OnClickButtonSwitchToRegisterTap()
    {
        logInTap.SetActive(false);
        registerTap.SetActive(true);
    }

    
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Test");
        }
    }
}