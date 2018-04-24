using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicActionManager : MonoBehaviour, IActionManager, ISSActionCallback
{

    public FirstSceneControl sceneController;
    public int diskNumber = 0;


    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();        
    private List<SSAction> waitingAdd = new List<SSAction>();                           
    private List<int> waitingDelete = new List<int>();                                 

    protected void Start()
    {
        sceneController = (FirstSceneControl)Director.getInstance().currentSceneControl;
        sceneController.actionManager = this;
    }

    protected void FixedUpdate()
    {
       
        foreach (SSAction a in waitingAdd)
            actions[a.GetInstanceID()] = a;
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction a = kv.Value;
            if (a.destroy)
            {
                waitingDelete.Add(a.GetInstanceID());
            }
            else if (a.enable)
            {
                a.FixedUpdate();
            }
        }
        
        foreach (int key in waitingDelete)
        {
            SSAction a = actions[key];
            actions.Remove(key);
            DestroyObject(a);
        }
        waitingDelete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }


    public void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0,
        string strParam = null,
        UnityEngine.Object objectParam = null)
    {
        if (source is CCFlyAction)
        {
            diskNumber--;
            source.gameobject.SetActive(false);
        }
    }

    public void beginThrow(Queue<GameObject> diskQueue)
    {
        CCFlyActionFactory cf = Singleton<CCFlyActionFactory>.Instance;
        foreach (GameObject tmp in diskQueue)
        {
            RunAction(tmp, cf.GetSSAction(), (ISSActionCallback)this);
        }
    }

    public int getdiskNumber()
    {
        return diskNumber;
    }

    public void setdiskNumber(int num)
    {
        diskNumber = num;
    }
}
