using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyAction : SSAction
{
    //重力加速度
    float gravity;
    //水平方向速度
    float xSpeed;
    //飞碟的初始方向
    Vector3 initDir;
    //飞碟已经飞行的时间
    float movingTime;
    //飞碟的刚体属性
    Rigidbody rigidbody;
    //飞碟的属性
    DiskData disk;

    public override void Start()
    {
        disk = gameobject.GetComponent<DiskData>();
        enable = true;
        gravity = 9.8f;
        movingTime = 0;
        xSpeed = disk.speed;
        initDir = disk.direction;

        rigidbody = this.gameobject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.velocity = xSpeed * initDir;
        }

    }

    public override void Update()
    {
        if (gameobject.activeSelf)
        {
            movingTime += Time.deltaTime;
            
            //水平方向的位移
            transform.Translate(initDir * xSpeed * Time.deltaTime);

            //竖直方向的位移
            transform.Translate(Vector3.down * gravity * movingTime * Time.deltaTime);

            //若飞碟飞出屏幕回收
            if (this.transform.position.y < -10)
            {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }

    }


    public override void FixedUpdate()
    {

        if (gameobject.activeSelf)
        {
            if (this.transform.position.y < -10)
            {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }
    }

    public static CCFlyAction GetCCFlyAction()
    {
        CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction>();
        return action;
    }
}