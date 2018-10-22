using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using Newtonsoft.Json;
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



    public void CreateMatchMakingRequest(Action<String, String> success = null, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            return;
        }
        ChallengeStartedMessage.Listener = (message) =>
        {
            success(message.Challenge.ChallengeId, message.Challenge.NextPlayer);
            // We only want to listen to this once
            ClearStartedMessageListener();
        };
        new MatchmakingRequest().SetMatchShortCode("DefaultMatch").SetSkill(0).Send((response) =>
        {
        }, (error) =>
        {
            Debug.LogError("Error Creating MatchMaking Request");
            ClearStartedMessageListener();
            GSData errors = error.Errors;
            Debug.LogError(errors.JSON);
            failure();
        });
    }

    public void CancelMatchMaking(Action success = null, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            return;
        }
        ClearStartedMessageListener();
        new MatchmakingRequest().SetAction("cancel").SetMatchShortCode("DefaultMatch").Send((response) =>
        {
            success();
        }, (error) =>
        {
            Debug.LogError("Error Creating MatchMaking Request");
            failure();
        });
    }

    private void ClearStartedMessageListener()
    {
        ChallengeStartedMessage.Listener = (m) => { };
    }

    public void MakeMove(string challengeId, Move move, Action success = null, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            return;
        }
        string moveJson = JsonConvert.SerializeObject(move);
        GSRequestData moveData = new GSRequestData(moveJson);
        Debug.Log(moveJson);
        new LogChallengeEventRequest()
        .SetChallengeInstanceId(challengeId)
        .SetEventKey("MakeMove")
        .SetEventAttribute("move", moveData).Send((response) =>
         {
             Debug.Log("Sent Move To Server");
             success();
         }, (error) =>
         {
             Debug.LogError("Error Sending Move To Server");
             Debug.LogError(error.Errors.JSON);
             failure();
         });
    }

    public void ForfeitGame(string challengeId, Action success = null, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            return;
        }
        new LogEventRequest()
        .SetEventAttribute("challengeInstanceId", challengeId)
        .SetEventKey("ForfeitGame")
        .Send((response) =>
         {
             Debug.Log("Sent Forfeit To Server");
             success();
         }, (error) =>
         {
             Debug.LogError("Error Sending Forfeit To Server");
             Debug.LogError(error.Errors.JSON);
             failure();
         });
    }
    public void SetMoveHandler(Action<Move[]> success = null, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            return;
        }
        ChallengeTurnTakenMessage.Listener = (response) =>
        {
            Debug.Log("Getting Move From Handler");
            List<GSData> data = response.Challenge.ScriptData.GetGSDataList("BOARD");
            List<Move> moves = new List<Move>();
            foreach (var rawMove in data)
            {
                Move move = JsonConvert.DeserializeObject<Move>(rawMove.JSON);
                moves.Add(move);
            }
            success(moves.ToArray());
        };
    }

    public void SetForfeitHandler(Action success = null, Action failure = null)
    {
        if (!Managers.Auth.loggedIn)
        {
            Debug.LogWarning("User Not Authenticated");
            return;
        }
        ChallengeWonMessage.Listener = (response) =>
        {
            Debug.Log("Winner leaving");
            success();
        };
        ChallengeLostMessage.Listener = (response) =>
        {
            Debug.Log("Loser leaving");
            success();
        };
    }


}