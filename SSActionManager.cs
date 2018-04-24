using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour
{

    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();      
    private List<SSAction> waitingAdd = new List<SSAction>();                           
    private List<int> waitingDelete = new List<int>();                                 

    protected void Start()
    {
    }

    protected void Update()
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
                a.Update();
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
}