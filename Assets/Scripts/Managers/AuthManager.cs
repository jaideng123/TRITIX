using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Facebook.Unity;
using UnityEngine;

public class AuthManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status
    {
        get; private set;
    }

    private CognitoSyncManager syncManager;
    private List<string> perms;

    public bool loggedIn { get; private set; }



    private const string COGNITO_IDENTITY_POOL_ID = "us-west-2:0db09d47-989a-458a-8dbe-b58e96f4e8b2";
    private CognitoAWSCredentials _credentials;
    public CognitoAWSCredentials credentials
    {
        get
        {
            if (_credentials == null)
                _credentials = new CognitoAWSCredentials(COGNITO_IDENTITY_POOL_ID, RegionEndpoint.USWest2);
            return _credentials;
        }
    }

    public void Startup()
    {
        Debug.Log("Starting Auth Manager");
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        perms = new List<string>() { "public_profile", "email" };
        FB.Init(delegate ()
        {
            if (FB.IsLoggedIn)
            { //User already logged in from a previous session
                AddFacebookTokenToCognito();
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
            return Facebook.Unity.AccessToken.CurrentAccessToken.UserId;
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
        credentials.Clear();
        credentials.ClearCredentials();
        credentials.ClearIdentityCache();
        loggedIn = false;
    }

    private void AddFacebookTokenToCognito()
    {
        loggedIn = true;
        credentials.AddLogin("graph.facebook.com", Facebook.Unity.AccessToken.CurrentAccessToken.TokenString);

    }

    private void SyncUserInfo()
    {
        if (loggedIn)
        {
            syncManager = new CognitoSyncManager(
                    credentials,
                    new AmazonCognitoSyncConfig
                    {
                        RegionEndpoint = RegionEndpoint.USWest2 // Region
                    }
                );

            // Create a record in a dataset and synchronize with the server
            Dataset dataset = syncManager.OpenOrCreateDataset("authDataset");
            dataset.Put("FB_User_Id", Facebook.Unity.AccessToken.CurrentAccessToken.UserId);
            dataset.SynchronizeAsync();
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            AddFacebookTokenToCognito();
            // AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USWest2);
            // DynamoDBContext context = new DynamoDBContext(client);
            // PublicGame game = new PublicGame();
            // game.player1Id = Facebook.Unity.AccessToken.CurrentAccessToken.UserId;
            // context.SaveAsync(game, (db_result) =>
            // {
            //     if (db_result.Exception == null)
            //         Debug.Log("Saved Successfully");
            //     else
            //     {
            //         Debug.Log(db_result.Exception);
            //     }
            // });
            SyncUserInfo();
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