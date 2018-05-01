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


    // Use this for initialization
    void Start()
    {
        SetLoginButtons();
    }

    private void SetLoginButtons()
    {
        loginButton.gameObject.SetActive(!Managers.Auth.loggedIn);
        logoutButton.gameObject.SetActive(Managers.Auth.loggedIn);
    }

    // Update is called once per frame
    void Update()
    {
        SetLoginButtons();
    }
    public void FindOrCreatePublicGame()
    {
        Debug.Log("Finding Game!");
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
            game = new WaitForCallback<PublicGame>(
                done => Managers.Online.FindGameById(id, result => done(result))
            );
            yield return game;
        }
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
        Managers.Online.UpdateGame(game);

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

}
