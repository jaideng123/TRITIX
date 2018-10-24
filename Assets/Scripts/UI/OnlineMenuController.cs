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
    private Image loadingImage;
    [SerializeField]
    private Button quickGameButton;
    [SerializeField]
    private Button cancelSearchButton;
    [SerializeField]
    private Button backButton;
    private bool searching;

    private const float ACTIVE_WAIT_SECONDS = 10f;


    // Use this for initialization
    void Start()
    {
        SetInteractableElements();
    }

    private void SetInteractableElements()
    {
        loginButton.gameObject.SetActive(!Managers.Auth.loggedIn);
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
        Managers.Online.CreateMatchMakingRequest(MatchFound, OnCancelSearch);
    }
    private void MatchFound(string gameId, string hostId)
    {
        LoadGame(gameId, hostId);
    }

    public void OnCancelSearch()
    {
        searching = false;
        Managers.Online.CancelMatchMaking();
    }

    private void LoadGame(string gameId, string hostId)
    {
        int activePlayer = Managers.Auth.userId == hostId ? 1 : 2;
        var param = new Dictionary<string, string>()
        {
            { "local-player", activePlayer.ToString()},
            { "game-id",gameId }
        };
        searching = false;
        Managers.GameMode.StartGame(GameMode.ONLINE, param);
    }

    public void Login()
    {
        Managers.Auth.Login();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application Closed");
        OnCancelSearch();
    }

}
