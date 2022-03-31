using System.Collections;
using System.Collections.Generic;
using _MMORPG.Scripts.Authentication;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Authenticate");
        }
    }
    [ContextMenu("Test2")]
    private void Test2()
    {
        PlayfabAuthHandler.Test("AAA");
    }
}
