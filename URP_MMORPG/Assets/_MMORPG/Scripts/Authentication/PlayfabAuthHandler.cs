using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Facebook.Unity;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Networking;
using LoginResult = PlayFab.ClientModels.LoginResult;

namespace _MMORPG.Scripts.Authentication
{
    /// <summary>
    /// Authentication using Playfab
    /// </summary>
    internal static class PlayfabAuthHandler
    {
        internal static event Action<string> OnSendMessage;

        private const string TitleID = "5D8B6"; // You can find this in your Playfab project.

        #region Register

        /// <summary>
        /// Register user with Playfab using userName, email and password.
        /// </summary>
        /// <param name="name"> User's name </param>
        /// <param name="email"> User's email </param>
        /// <param name="password"> User's password </param>
        internal static void Register(string name, string email, string password)
        {
            // Do it when the IsCheckRegisterInput returns a false
            if (!IsCheckRegisterInput(name, email, password))
            {
                return;
            }

            var request = new RegisterPlayFabUserRequest
            {
                Username = name,
                Email = email,
                Password = Encrypt(password)
            };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterError);
        }

        private static void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            HandleSendMessage("Register Success!");
        }

        private static void OnRegisterError(PlayFabError error)
        {
            string message;

            switch (error.Error)
            {
                case PlayFabErrorCode.UsernameNotAvailable:
                    message = "Username not available!";
                    break;
                case PlayFabErrorCode.EmailAddressNotAvailable:
                    message = "Email address not available!";
                    break;
                default:
                    message = $"Error: {error.Error}!";
                    break;
            }

            HandleSendMessage(message);
        }

        #endregion

        #region Login

        /// <summary>
        /// Login user with EmailAddress using email and password.
        /// </summary>
        /// <param name="email"> User's email </param>
        /// <param name="password"> User's password </param>
        internal static void Login(string email, string password)
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = Encrypt(password)
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginError);
        }

        private static void OnLoginSuccess(LoginResult result)
        {
            Debug.Log($"Login");
        }

        private static void OnLoginError(PlayFabError error)
        {
            Debug.Log($"Error{error.Error}");
        }

        #region Login_With_Facebook

        /// <summary>
        /// Initialization Facebook.
        /// </summary>
        internal static void InitializationFacebook()
        {
            // Do  Do it when FB.IsInitialized Equals false
            if (!FB.IsInitialized)
            {
                FB.Init(FB.ActivateApp);
            }
            else
            {
                FB.ActivateApp();
            }
        }

        /// <summary>
        /// Permissions login facebook
        /// </summary>
        internal static void LoginWithFacebook()
        {
            List<string> permissions = new List<string> {"public_profile", "email"};

            FB.LogInWithReadPermissions(permissions, FacebookAuthCallback);
        }

        /// <summary>
        /// Callback result FB.LogInWithReadPermissions
        /// </summary>
        /// <param name="result"> result </param>
        private static void FacebookAuthCallback(ILoginResult result)
        {
            if (result.Error != null)
            {
                Debug.Log($"FB Error: {result.Error}");
                HandleSendMessage($"FB Error: {result.Error}");
            }
            else if (result.Cancelled)
            {
                Debug.Log($"Facebook Login Cancelled");
                HandleSendMessage($"Facebook Login Cancelled");
            }
            else if (FB.IsLoggedIn)
            {
                var accessToken = AccessToken.CurrentAccessToken.TokenString;

                // CheckDebug
                Debug.Log($"FB get:{accessToken}");

                FacebookLogin(accessToken);
            }
        }

        /// <summary>
        /// Login user with facebook. 
        /// </summary>
        /// <param name="accessToken"> AccessToken's FacebookAuthCallback </param>
        private static void FacebookLogin(string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest
                {
                    AccessToken = accessToken,
                    CreateAccount = true
                }, OnFBLoginSuccess, OnFBLoginError);
            }
        }

        private static void OnFBLoginSuccess(LoginResult result)
        {
            Debug.Log("FB Login Success");
            HandleSendMessage($"FB Login Success");
        }

        private static void OnFBLoginError(PlayFabError error)
        {
            Debug.Log($"FB Login error: {error.Error}");
            HandleSendMessage($"FB Login error: {error.Error}");
        }

        #endregion

        #endregion

        #region ResetPassword

        /// <summary>
        /// Reset password using email.
        /// </summary>
        /// <param name="email"> User's email </param>
        internal static void ResetPassword(string email)
        {
            var request = new SendAccountRecoveryEmailRequest
            {
                Email = email,
                TitleId = TitleID
            };
            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnSendEmailSuccess, OnSendEmailError);
        }

        private static void OnSendEmailSuccess(SendAccountRecoveryEmailResult result)
        {
        }

        private static void OnSendEmailError(PlayFabError error)
        {
        }

        #endregion

        #region Test

        internal static void Test(string displayName)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = displayName
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, TestSuccess, TestError);
        }

        private static void TestSuccess(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log(result.DisplayName);
        }

        private static void TestError(PlayFabError error)
        {
            Debug.Log($"Error {error.Error}");
        }

        #endregion

        /// <summary>
        /// Encrypted password is used for registration and login.
        /// </summary>
        /// <param name="pass"> User's password </param>
        /// <returns></returns>
        private static string Encrypt(string pass)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(pass);
            bs = x.ComputeHash(bs);
            StringBuilder s = new StringBuilder();

            foreach (var b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }

            return s.ToString();
        }

        /// <summary>
        /// Receive a message to display
        /// </summary>
        /// <param name="message"> The message will be displayed </param>
        private static void HandleSendMessage(string message)
        {
            OnSendMessage?.Invoke(message);
        }

        /// <summary>
        /// Verify that the information received is correct?. Returns a boolean.
        /// </summary>
        /// <param name="name"> User's name </param>
        /// <param name="email"> User's email </param>
        /// <param name="password"> User's password </param>
        /// <returns></returns>
        private static bool IsCheckRegisterInput(string name, string email, string password)
        {
            var result = true;

            // Do it when the username is less than 6 characters or empty.
            if (name.Length < 6 || string.IsNullOrWhiteSpace(name))
            {
                HandleSendMessage("Username must be greater than or equal to 6 characters and not empty!");
                result = false;
            }
            // Do it when the email is empty.
            else if (string.IsNullOrEmpty(email))
            {
                HandleSendMessage("Email cannot be empty!");
                result = false;
            }
            // Do it when the password is less than 6 characters or empty.
            else if (password.Length < 6 || string.IsNullOrWhiteSpace(password))
            {
                HandleSendMessage("Password must be greater than or equal to 6 characters and not empty!");
                result = false;
            }

            return result;
        }
    }
}