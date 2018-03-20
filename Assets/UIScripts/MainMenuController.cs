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

    public void OnGameModeSelect(string modeString)
    {
        GameMode mode = (GameMode)Enum.Parse(typeof(GameMode), modeString);
        Managers.GameMode.StartGame(mode);
    }

}
