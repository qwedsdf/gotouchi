using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public void GoToAvatarScene()
    {
        SceneFadeManager.Instance.Load(GameData.Scene_Avatar, GameData.FadeSpeed);
    }

    public void GoToBattleScene()
    {
        SceneFadeManager.Instance.Load(GameData.Scene_Battle, GameData.FadeSpeed);
    }
}
