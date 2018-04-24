using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CCFlyActionFactory : MonoBehaviour
{
    private Dictionary<int, SSAction> beUsed = new Dictionary<int, SSAction>();
    private List<SSAction> free = new List<SSAction>();
    private List<int> wait = new List<int>();

    public CCFlyAction FlyAction;

    void Start()
    {
        FlyAction = CCFlyAction.GetCCFlyAction();
    }


    private void Update()
    {
        foreach (var obj in beUsed.Values)
        {
            if (obj.destroy)
            {
                wait.Add(obj.GetInstanceID());
            }
        }

        foreach (int obj in wait)
        {
            FreeSSAction(beUsed[obj]);
        }
        wait.Clear();
    }

    public SSAction GetSSAction()
    {
        SSAction action = null;
        if (free.Count > 0)
        {
            action = free[0];
            free.Remove(free[0]);
            Debug.Log(free.Count);
        }
        else
        {
            action = ScriptableObject.Instantiate<CCFlyAction>(FlyAction);

        }

        beUsed.Add(action.GetInstanceID(), action);
        return action;
    }

    public void FreeSSAction(SSAction action)
    {
        SSAction obj = null;
        int key = action.GetInstanceID();
        if (beUsed.ContainsKey(key))
        {
            obj = beUsed[key];
        }

        if (obj != null)
        {
            obj.reset();
            free.Add(obj);
            beUsed.Remove(key);
        }
    }

    public void clear()
    {
        foreach (var obj in beUsed.Values)
        {
            obj.enable = false;
            obj.destroy = true;

        }
    }
}