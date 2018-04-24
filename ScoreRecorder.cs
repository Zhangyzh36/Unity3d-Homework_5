using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreRecorder : MonoBehaviour
{
    public int score;
    private Dictionary<Color, int> scoreMap = new Dictionary<Color, int>();
  
    void Start()
    {
        scoreMap.Add(Color.yellow, 1);
        scoreMap.Add(Color.red, 3);
        scoreMap.Add(Color.blue, 5);

        score = 0;
    }

    public void Record(GameObject disk)
    {
        score += scoreMap[disk.GetComponent<DiskData>().color];
    }

    public void Reset()
    {
        score = 0;
    }
}