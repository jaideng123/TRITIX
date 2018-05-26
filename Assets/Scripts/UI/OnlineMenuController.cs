using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineMenuController : MonoBehaviour
{
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button logoutButton;
    [SerializeField]
    private Image loadingImage;
    [SerializeField]
    private Button quickGameButton;
    [SerializeField]
    private Button cancelSearchButton;
    [SerializeField]
    private Button backButton;
    private bool searching;

    private string openGameId;


    // Use this for initialization
    void Start()
    {
        SetInteractableElements();
        openGameId = null;
    }

    private void SetInteractableElements()
    {
        loginButton.gameObject.SetActive(!Managers.Auth.loggedIn);
        logoutButton.gameObject.SetActive(Managers.Auth.loggedIn);
        quickGameButton.gameObject.SetActive(!searching);
        quickGameButton.interactable = Managers.Auth.loggedIn;
        cancelSearchButton.gameObject.SetActive(searching);
        backButton.gameObject.SetActive(!searching);
        loadingImage.gameObject.SetActive(searching);
    }

    // Update is called once per frame
    void Update()
    {
        SetInteractableElements();
    }
    public void FindOrCreatePublicGame()
    {
        Debug.Log("Finding Game!");
        searching = true;
        Managers.Online.FindOpenGame(OnFindRecieved);
    }

    private void OnFindRecieved(List<String> gameIds)
    {
        Debug.Log("Find Recieved");
        Debug.Log(gameIds.Count);
        if (gameIds.Count > 0)
        {
            Managers.Online.FindGameById(gameIds[0], JoinGame);
        }
        else
        {
            Managers.Online.CreateNewGame(OnGameCreated);
        }
        // foreach (string id in gameIds)
        // {
        //     Managers.Online.FindGameById(id, PrintGame);
        // }
    }
    private void OnGameCreated(string id)
    {
        Debug.Log("Created Game: " + id);
        openGameId = id;
        StartCoroutine(WaitForGameJoin(id));
    }

    private IEnumerator WaitForGameJoin(string id)
    {
        Debug.Log("Waiting for game join...");
        var game = new WaitForCallback<PublicGame>(
            done => Managers.Online.FindGameById(id, result => done(result))
        );
        yield return game;
        while (game.Result.player2Id == null)
        {
            Debug.Log("Still Waiting...");
            yield return new WaitForSeconds(1);
            game = new WaitForCallback<PublicGame>(
                done => Managers.Online.FindGameById(id, result => done(result))
            );
            yield return game;
        }
        LoadGame(game.Result);
        Debug.Log("Player Found!");
    }

    private void JoinGame(PublicGame game)
    {
        if (game.player2Id != null)
        {
            Debug.LogWarning("Game " + game.id + " Is already full");
            return;
        }
        game.player2Id = Managers.Auth.GetUserId();
        Managers.Online.UpdateGame(game, LoadGame, FindOrCreatePublicGame);
    }

    private void LoadGame(PublicGame game)
    {
        int activePlayer = game.player1Id == Managers.Auth.GetUserId() ? 1 : 2;
        var param = new Dictionary<string, string>()
        {
            { "local-player", activePlayer.ToString()},
            { "game-id",game.id }
        };
        searching = false;
        game.active = true;
        Managers.Online.UpdateGame(game);
        Managers.GameMode.StartGame(GameMode.ONLINE, param);
    }

    public void OnCancelSearch()
    {
        searching = false;
        if (openGameId != null)
        {
            Managers.Online.DeleteGame(openGameId);
            openGameId = null;
        }
    }


    private void PrintGame(PublicGame game)
    {
        Debug.Log(game.player1Id);
    }

    public void Login()
    {
        Managers.Auth.Login();
    }

    public void LogOut()
    {
        Managers.Auth.LogOut();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application Closed");
        OnCancelSearch();
    }

}
