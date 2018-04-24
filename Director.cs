using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{

    /** 
     * currentSceneControl标志目前正在使用的场景 
     */

    public ISceneControl currentSceneControl { get; set; }

    /** 
     * Director这个类是采用单例模式 
     */

    private static Director director;

    private Director()
    {

    }

    public static Director getInstance()
    {
        if (director == null)
        {
            director = new Director();
        }
        return director;
    }
}
