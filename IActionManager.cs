using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionManager
{
    void beginThrow(Queue<GameObject> diskQueue);
    int getdiskNumber();
    void setdiskNumber(int num);
}