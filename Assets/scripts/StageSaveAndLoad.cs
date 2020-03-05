using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSaveAndLoad
{
    // Start is called before the first frame update
    public StageManager stageManager;
    public int stageIndex_scene0;
    public int stageIndex_scene1;
    public static int stageIndex;
    public static int MaxStage = 12;
    public static int thisStage = 0;
    public static int saveStage = 0;


    // Start is called before the first frame update
    void Awake()
    {
        //MaxStage = 11;
        //DontDestroyOnLoad(gameObject);
    }
    public void StageSelete()
    {
        stageIndex = stageManager.moveStageIndex;
        stageIndex_scene1 = stageIndex;
        Debug.Log(stageIndex_scene1);
    }
    public int StageReturn()
    {
        Debug.Log("StageReturn"+(stageIndex));
        return stageIndex;
    }
}
