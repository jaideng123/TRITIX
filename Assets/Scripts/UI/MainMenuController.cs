using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [System.Serializable]
    public class MenuPanel
    {
        public string name;
        public HideablePanel panel;
    }
    [SerializeField]
    private MenuPanel[] menuPanels;
    private MenuPanel activePanel = null;

    void Awake()
    {
        foreach (MenuPanel panel in menuPanels)
        {
            if (panel.panel)
            {
                panel.panel.SetActive(false);
            }
        }
        activePanel = menuPanels[0];
        activePanel.panel.SetActive(true);
    }

    public void openMenu(string menuName)
    {
        for (int i = 0; i < menuPanels.Length; i++)
        {
            if (menuPanels[i].name == menuName)
            {
                activePanel.panel.SetActive(false);
                activePanel = menuPanels[i];
                activePanel.panel.SetActive(true);
                return;
            }
        }
        Debug.LogWarning("No Panel Found for: " + menuName);
    }

    public void OnGameModeSelect(string modeString)
    {
        GameMode mode = (GameMode)Enum.Parse(typeof(GameMode), modeString);
        Managers.GameMode.StartGame(mode);
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

}
