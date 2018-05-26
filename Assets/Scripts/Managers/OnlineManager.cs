using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using UnityEngine;
// TODO consider refactoring to async/await
public class OnlineManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status
    {
        get; private set;
    }

    public void Startup()
    {
        Debug.Log("Starting Online Manager");
        status = ManagerStatus.Started;
    }



    private Action doNothing = () => { };



    public void CreateNewGame(Action<string> success = null, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            return;
        }
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(Managers.Auth.credentials, RegionEndpoint.USWest2);
        DynamoDBContext context = new DynamoDBContext(client);
        PublicGame game = new PublicGame();
        Move move = new Move();
        move.to = new Vector3Int(0, 0, 0);
        game.moves.Add(move);
        game.player1Id = Managers.Auth.GetUserId();
        context.SaveAsync(game, (result) =>
        {
            if (result.Exception != null)
            {
                Debug.Log(result.Exception);
                if (failure != null)
                {
                    failure();
                }
                return;
            }
            Debug.Log("Saved Successfully");
            if (success != null)
            {
                success(game.id);
            }
        });
    }

    public void FindOpenGame(Action<List<String>> success, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            if (failure != null)
            {
                failure();
            }
            return;
        }
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(Managers.Auth.credentials, RegionEndpoint.USWest2);
        DynamoDBContext context = new DynamoDBContext(client);
        // var client = new AmazonLambdaClient(Managers.Auth.credentials, RegionEndpoint.USWest2);
        // var request = new InvokeRequest()
        // {
        //     FunctionName = "FindOpenGames",
        //     Payload = "{}",
        //     InvocationType = InvocationType.RequestResponse
        // };
        // Debug.Log("Searching Lambda");
        // client.InvokeAsync(request, (result) =>
        // {
        //     List<string> openGames = new List<string>();
        //     if (result.Exception == null)
        //     {

        //         Debug.Log(Encoding.ASCII.GetString(result.Response.Payload.ToArray()));
        //         success(openGames);
        //     }
        //     else
        //     {
        //         Debug.LogError(result.Exception);
        //     }
        // });
        // This didn't work but it should have
        // AsyncSearch<PublicGame> search = context.ScanAsync<PublicGame>(new ScanCondition("player1Id", ScanOperator.Equal, "10209697164105086"));
        // Debug.Log("Got Search");
        // Debug.Log(search.IsDone);
        // search.GetNextSetAsync((result) =>
        // {
        //     if (result.Exception == null)
        //     {
        //         foreach (PublicGame gri in result.Result)
        //         {
        //             Debug.Log("query gri: " + gri.id);
        //         }
        //     }
        // });

        var request = new ScanRequest
        {
            TableName = "PublicGame",
            //TODO Limit this search
            FilterExpression = "attribute_not_exists(player2Id)",
            ProjectionExpression = "id"
        };

        client.ScanAsync(request, (result) =>
        {
            List<string> openGames = new List<string>();
            if (result.Exception != null)
            {
                Debug.Log(result.Exception);
                return;
            }
            Debug.Log(result);
            foreach (Dictionary<string, AttributeValue> item
                     in result.Response.Items)
            {
                foreach (var it in item.Keys)
                {
                    openGames.Add(item[it].S);
                    Debug.Log(item[it].S);
                }
            }
            success(openGames);
        });
    }

    public void FindGameById(string id, Action<PublicGame> success, Action failure = null)
    {
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(Managers.Auth.credentials, RegionEndpoint.USWest2);
        DynamoDBContext context = new DynamoDBContext(client);
        Debug.Log("Finding Game");
        context.LoadAsync<PublicGame>(id, (result) =>
            {
                if (result.Exception != null)
                {
                    Debug.Log(result.Exception);
                    if (failure != null)
                    {
                        failure();
                    }
                    return;
                }
                PublicGame game = result.Result as PublicGame;
                success(game);
            });
    }

    public void UpdateGame(PublicGame game, Action<PublicGame> success = null, Action failure = null)
    {
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(Managers.Auth.credentials, RegionEndpoint.USWest2);
        DynamoDBContext context = new DynamoDBContext(client);
        context.SaveAsync<PublicGame>(game, (res) =>
            {
                if (res.Exception != null)
                {
                    Debug.Log(res.Exception);
                    if (failure != null)
                    {
                        failure();
                    }
                    return;
                }
                if (success != null)
                {
                    success(game);
                }
            });
    }

    public void DeleteGame(string gameId, Action success = null, Action failure = null)
    {
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(Managers.Auth.credentials, RegionEndpoint.USWest2);
        DynamoDBContext context = new DynamoDBContext(client);
        context.DeleteAsync<PublicGame>(gameId, (res) =>
            {
                if (res.Exception != null)
                {
                    Debug.Log(res.Exception);
                    if (failure != null)
                    {
                        failure();
                    }
                    return;
                }
                if (success != null)
                {
                    success();
                }
            });
    }

    public void DeleteInactiveGames(string gameId, Action success = null, Action failure = null)
    {
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(Managers.Auth.credentials, RegionEndpoint.USWest2);
        DynamoDBContext context = new DynamoDBContext(client);

    }

    public void FindActiveGames(Action<List<String>> success = null, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            if (failure != null)
            {
                failure();
            }
            return;
        }
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(Managers.Auth.credentials, RegionEndpoint.USWest2);
        DynamoDBContext context = new DynamoDBContext(client);
        var request = new ScanRequest
        {
            TableName = "PublicGame",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                        {":id", new AttributeValue { S = Managers.Auth.GetUserId() }},
                        {":active",new AttributeValue { N = "1" }}
},
            FilterExpression = "(player1Id = :id or player2Id = :id) and (active = :active)",
            ProjectionExpression = "id",
            ConsistentRead = true
        };

        client.ScanAsync(request, (result) =>
        {
            List<string> openGames = new List<string>();
            if (result.Exception != null)
            {
                Debug.Log(result.Exception);
                return;
            }
            foreach (Dictionary<string, AttributeValue> item
                     in result.Response.Items)
            {
                foreach (var it in item.Keys)
                {
                    openGames.Add(item[it].S);
                    Debug.Log(item[it].S);
                }
            }
            success(openGames);
        });
    }



}