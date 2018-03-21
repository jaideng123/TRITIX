using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackdropManager : MonoBehaviour, IGameManager
{
    public Backdrop currentBackdrop { get; private set; }
    [SerializeField]
    public Backdrop[] availableBackdrops;
    private GameObject backdropObject;
    public ManagerStatus status
    {
        get; private set;
    }

    public void Startup()
    {
        Debug.Log("Starting Backdrop Manager");
        LoadBackdrop(availableBackdrops[0]);
        status = ManagerStatus.Started;
    }

    public void LoadBackdrop(Backdrop backdrop)
    {
        GetComponent<Fading>().BeginFade(1);
        if (backdropObject != null)
        {
            Destroy(backdropObject);
        }
        currentBackdrop = backdrop;
        backdropObject = Instantiate(currentBackdrop.scenePrefab, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(backdropObject);
        RenderSettings.skybox = currentBackdrop.skyBox;
        GetComponent<Fading>().BeginFade(-1);
    }


}