using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneControl : MonoBehaviour, ISceneControl, IUserAction
{
    public ActionMode mode { get; set; }
    public IActionManager actionManager { get; set; }
    public ScoreRecorder scoreRecorder { get; set; }
    public Queue<GameObject> diskQueue = new Queue<GameObject>();
    private int diskNumber;
    private int curMode = -1;
    public int round = 3;
    private float time = 0;
    private GameState gameState = GameState.START;

    void Awake()
    {
        diskNumber = 10;

        Director director = Director.getInstance();
        director.currentSceneControl = this;
        
        this.gameObject.AddComponent<ScoreRecorder>();
        this.gameObject.AddComponent<DiskFactory>();
        this.gameObject.AddComponent<CCFlyActionFactory>();
        mode = ActionMode.NONE;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;

        director.currentSceneControl.LoadResources();
    }

    private void Update()
    {
        Debug.Log(mode);
        if (mode != ActionMode.NONE && actionManager != null)
        {
            //飞碟数量为0并且在游戏中
            if (actionManager.getdiskNumber() == 0 && gameState == GameState.IN_GAME)
            {
                //本轮结束
                gameState = GameState.OUT_ROUND;

            }
            //飞碟数量为0并且一轮游戏开始
            if (actionManager.getdiskNumber() == 0 && gameState == GameState.IN_ROUND)
            {
                curMode = (curMode + 1) % round;
                NextRound();
                actionManager.setdiskNumber(10);
                gameState = GameState.IN_GAME;
            }

            //一个飞碟抛出后下一随机时刻抛出下一个飞碟
            if (time > Random.Range(0.5f,1.5f))
            {
                ThrowDisk();
                time = 0;
            }
            else
            {
                time += Time.deltaTime;
            }
        }

    }

    private void NextRound()
    {
        DiskFactory df = Singleton<DiskFactory>.Instance;
        for (int i = 0; i < diskNumber; i++)
        {
            diskQueue.Enqueue(df.GetDisk(curMode, mode));
        }

        actionManager.beginThrow(diskQueue);


    }

    void ThrowDisk()
    {
        if (diskQueue.Count != 0)
        {
            GameObject disk = diskQueue.Dequeue();
            //飞碟的高度为1~3之间的随机数
            float y = Random.Range(1f, 3f);
            disk.transform.position = new Vector3(-disk.GetComponent<DiskData>().direction.x * 7, y, 0);
            //激活飞碟
            disk.SetActive(true);
        }

    }

    public void LoadResources()
    {
        //空
    }


    public void GameOver()
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(700, 300, 400, 400), "游戏结束");
    }

    public void setGameState(GameState newState)
    {
        gameState = newState;
    }

    public int GetScore()
    {
        return scoreRecorder.score;
    }

    public GameState getGameState()
    {
        return gameState;
    }


    public void hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            //若击中飞碟
            if (hit.collider.gameObject.GetComponent<DiskData>() != null)
            {
                //记下相应的分数
                scoreRecorder.Record(hit.collider.gameObject);
                //飞碟瞬移产生消失的效果
                hit.collider.gameObject.transform.position = new Vector3(0, -10, 0);
            }

        }
    }

    public ActionMode getMode()
    {
        return mode;
    }

    public void setMode(ActionMode m)
    {
        if (m == ActionMode.KINEMATIC)
        {
            //选择运动模式，添加相应脚本并且设置模式为运动
            this.gameObject.AddComponent<CCActionManager>();
            mode = ActionMode.KINEMATIC;
        }
        else if (m == ActionMode.PHYSICS)
        { 
            //选择物理模式，添加相应脚本并且设置模式为物理
            this.gameObject.AddComponent<PhysicActionManager>();
            mode = ActionMode.PHYSICS;
        }
    }
}