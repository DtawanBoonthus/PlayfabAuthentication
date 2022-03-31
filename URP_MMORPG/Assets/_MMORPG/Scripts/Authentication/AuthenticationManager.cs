using System;
using System.Collections;
using System.Collections.Generic;
using _MMORPG.Scripts.Authentication;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class AuthenticationManager : MonoBehaviour
{
    [SerializeField] private TMP_Text message;
    [SerializeField] private TMP_InputField userNameRegister;
    [SerializeField] private TMP_InputField userEmailRegister;
    [SerializeField] private TMP_InputField userPasswordRegister;

    [SerializeField] private TMP_InputField userEmailLogin;
    [SerializeField] private TMP_InputField userPasswordLogin;

    [Header("Add Button")] 
    [SerializeField] private Button registerButton;
    [SerializeField] private Button logInButton;
    [SerializeField] private Button resetPasswordButton;
    [SerializeField] private Button loginWithFacebookButton;

    private void Awake()
    {
        PlayfabAuthHandler.InitializationFacebook();
        message.text = string.Empty;
    }

    private void Start()
    {
        registerButton.onClick.AddListener(OnClickRegisterButton);
        logInButton.onClick.AddListener(OnClickLogInButton);
        resetPasswordButton.onClick.AddListener(OnClickResetPasswordButton);
        loginWithFacebookButton.onClick.AddListener(OnClickLoginWithFacebookButton);

        PlayfabAuthHandler.OnSendMessage += ChangeMessage;
    }

    private void OnClickLoginWithFacebookButton()
    {
        PlayfabAuthHandler.LoginWithFacebook();
    }

    private void ChangeMessage(string changeMessage)
    {
        message.text = changeMessage;
    }

    /*private void OnClickRegisterButton(string userName, string email, string password)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Username = userName,
            Email = email,
            Password = password,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterError);
    }*/

    private void OnClickRegisterButton()
    {
        PlayfabAuthHandler.Register(userNameRegister.text, userEmailRegister.text, userPasswordRegister.text);
    }

    private void OnClickLogInButton()
    {
        PlayfabAuthHandler.Login(userEmailLogin.text, userPasswordLogin.text);
    }

    private void OnClickResetPasswordButton()
    {
    }
}