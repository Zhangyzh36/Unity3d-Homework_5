using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;
    bool init = true;

    void Start()
    {
        action = Director.getInstance().currentSceneControl as IUserAction;
    }

    private void OnGUI()
    {
        if (action.getMode() == ActionMode.NONE)
        {
            //模式选择
            if (GUI.Button(new Rect(Screen.width / 2 - 100, 100, 90, 90), "物理"))
            {
                action.setMode(ActionMode.PHYSICS);
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 10, 100, 90, 90), "运动"))
            {
                action.setMode(ActionMode.KINEMATIC);
            }
        }
        else
        {
            //鼠标左键点击事件
            if (Input.GetButtonDown("Fire1"))
            {

                Vector3 pos = Input.mousePosition;
                action.hit(pos);
            }

            //显示得分
            GUI.Label(new Rect(Screen.width / 2 - 25, 0, 50, 50), "得分:" + action.GetScore().ToString());

            //开始游戏
            if (init && GUI.Button(new Rect(Screen.width / 2 - 45, 100, 90, 90), "开始"))
            {
                init = false;
                action.setGameState(GameState.IN_ROUND);
            }
            //下一轮
            if (!init && action.getGameState() == GameState.OUT_ROUND && GUI.Button(new Rect(Screen.width / 2 - 45, 100, 90, 90), "下一轮"))
            {
                action.setGameState(GameState.IN_ROUND);
            }
        }
    }


}