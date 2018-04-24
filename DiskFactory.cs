using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    public GameObject diskPre;

    private Dictionary<int, DiskData> beUsed = new Dictionary<int, DiskData>();
    private List<DiskData> free = new List<DiskData>();
    private List<int> wait = new List<int>();

    private void Awake()
    {
        diskPre = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/disk"), Vector3.zero, Quaternion.identity);
        diskPre.SetActive(false);
    }

    private void Update()
    {

        foreach (var obj in beUsed.Values)
        {

            if (!obj.gameObject.activeSelf)
            {
                wait.Add(obj.GetInstanceID());
            }
        }

        //将beUsed里的飞碟free
        foreach (int obj in wait)
        {
            GameObject freeDisk = beUsed[obj].gameObject;
            DiskData d = null;
            foreach (DiskData i in beUsed.Values)
            {
                if (freeDisk.GetInstanceID() == i.gameObject.GetInstanceID())
                {
                    d = i;
                }
            }
            if (d != null)
            {
                d.gameObject.SetActive(false);
                free.Add(d);
                beUsed.Remove(d.GetInstanceID());
            }
        }
        wait.Clear();
    }

    public GameObject GetDisk(int round, ActionMode mode)
    {
        GameObject newDisk = null;

        if (free.Count > 0)
        {
            newDisk = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            newDisk = GameObject.Instantiate<GameObject>(diskPre, Vector3.zero, Quaternion.identity);
            newDisk.AddComponent<DiskData>();
        }

        int seed = 0;
        if (round == 1)
            seed = 150;
        if (round == 2)
            seed = 300;
        int randomColor = Random.Range(seed, round * 499);

        if (randomColor > 500)
        {
            round = 2;
        }
        else if (randomColor > 300)
        {
            round = 1;
        }
        else
        {
            round = 0;
        }

        DiskData diskdata = newDisk.GetComponent<DiskData>();

        if (round == 0)
        {
            diskdata.color = Color.yellow;
            diskdata.speed = 4.0f;
            newDisk.GetComponent<Renderer>().material.color = Color.yellow;
        }
         else if (round == 1)
        {
            diskdata.color = Color.red;
            diskdata.speed = 6.0f;
            newDisk.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (round == 2)
        {
            diskdata.color = Color.blue;
            diskdata.speed = 8.0f;
            newDisk.GetComponent<Renderer>().material.color = Color.blue;
        }

        float randomX = Random.Range(-1f, 1f) < 0 ? -1 : 1;
        diskdata.direction = new Vector3(randomX, 1, 0);

        //若选择物理模式，则添加刚体组件
        if (mode == ActionMode.PHYSICS)
        {
            newDisk.AddComponent<Rigidbody>();
        }

        beUsed.Add(diskdata.GetInstanceID(), diskdata); 
        newDisk.name = newDisk.GetInstanceID().ToString();
        return newDisk;
    }

   
}