using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackdropManager : MonoBehaviour, IGameManager
{
    public Backdrop currentBackdrop { get; private set; }
    [SerializeField]
    public Backdrop blankBackdrop;
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
        SceneManager.activeSceneChanged += OnSceneChange;
        LoadDefaultBackdrop();
        status = ManagerStatus.Started;
    }

    public void LoadBlankBackdrop()
    {
        LoadBackdrop(blankBackdrop);
    }

    public void LoadDefaultBackdrop()
    {
        LoadBackdrop(availableBackdrops[0]);
    }

    public int getBackdropIndex(Backdrop backdrop)
    {
        return availableBackdrops.ToList().IndexOf(backdrop);
    }

    public void LoadBackdrop(Backdrop backdrop)
    {
        GetComponent<Fading>().BeginFade(1);
        if (backdropObject != null)
        {
            Destroy(backdropObject);
        }
        currentBackdrop = backdrop;
        if (currentBackdrop.scenePrefab != null)
        {
            backdropObject = Instantiate(currentBackdrop.scenePrefab, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(backdropObject);
        }
        RenderSettings.skybox = currentBackdrop.skyBox;
        GetComponent<Fading>().BeginFade(-1);
    }

    private void OnSceneChange(Scene prev, Scene current)
    {
        RenderSettings.skybox = currentBackdrop.skyBox;
    }

}