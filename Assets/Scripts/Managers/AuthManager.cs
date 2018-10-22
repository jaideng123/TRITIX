using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Facebook.Unity;
using GameSparks.Api.Requests;
using GameSparks.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status
    {
        get; private set;
    }
    private List<string> perms;

    public bool loggedIn { get; private set; }

    public string userId { get; private set; }

    public string displayName { get; private set; }

    public string authToken { get; private set; }



    public void Startup()
    {
        Debug.Log("Starting Auth Manager");
        perms = new List<string>() { "public_profile", "email" };
        FB.Init(delegate ()
        {
            if (FB.IsLoggedIn)
            { //User already logged in from a previous session
                LoginToGameSparks();
                Debug.Log("User logged in");
            }
            else
            {
                Debug.Log("User not logged in");
            }
        });
        status = ManagerStatus.Started;
    }

    public void Login()
    {
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    public string GetUserId()
    {
        if (FB.IsLoggedIn)
        {
            return Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
        }
        else
        {
            Debug.LogWarning("User Is Not Logged In!");
            return "";
        }
    }

    public void LogOut()
    {
        FB.LogOut();
        loggedIn = false;
    }

    private void LoginToGameSparks()
    {
        new FacebookConnectRequest()
                .SetAccessToken(Facebook.Unity.AccessToken.CurrentAccessToken.TokenString)
                .SetSyncDisplayName(true)
                .Send((response) =>
                {
                    authToken = response.AuthToken;
                    displayName = response.DisplayName;
                    userId = response.UserId;
                    Debug.Log(response.HasErrors);
                    loggedIn = true;
                }, (error) =>
                {
                    Debug.LogError("ERROR Connecting to gamesparks");
                    GSData errors = error.Errors;
                    Debug.LogError(errors.JSON);
                });
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            LoginToGameSparks();
            // Print current access token's granted permissions
            foreach (string perm in Facebook.Unity.AccessToken.CurrentAccessToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }


}