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

    public void Startup()
    {
        Debug.Log("Starting Auth Manager");
        FB.Init();
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        status = ManagerStatus.Started;
    }

    public void Login()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }




    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);

            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                    "us-west-2:0db09d47-989a-458a-8dbe-b58e96f4e8b2", // Identity pool ID
                    RegionEndpoint.USWest2 // Region
                );
            credentials.AddLogin("graph.facebook.com", aToken.TokenString);
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USWest2);
            DynamoDBContext context = new DynamoDBContext(client);
            PublicGame game = new PublicGame();
            game.player1Id = aToken.UserId;
            context.SaveAsync(game, (db_result) =>
            {
                if (db_result.Exception == null)
                    Debug.Log("Saved Successfully");
                else
                {
                    Debug.Log(db_result.Exception);
                }
            });


            // CognitoSyncManager syncManager = new CognitoSyncManager(
            //     credentials,
            //     new AmazonCognitoSyncConfig
            //     {
            //         RegionEndpoint = RegionEndpoint.USWest2 // Region
            //     }
            // );

            // // Create a record in a dataset and synchronize with the server
            // Dataset dataset = syncManager.OpenOrCreateDataset("myDataset");
            // dataset.Put("myKey", "myValue");
            // dataset.SynchronizeAsync();

            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
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