using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour{

    public GameObject[] UIStageBtn;
    public GameObject UIPageUp;
    public GameObject UIPageDown;
    public SceneChange sceneChange;
    //public StageSaveAndLoad stageSaveAndLoad;


    public int stageIndex;

    public int maxStage;
    //public int[] completeStage;
    public int moveStageIndex;
    public int page;

    void Start()
    {

    }
    void Awake()
    {
        
        maxStage = StageSaveAndLoad.MaxStage;
    }
    public void Update()
    {
        for (int i = 0; i < UIStageBtn.Length; i++)
        {
            if (maxStage < i + 1 + page * 8)
            {

                UIStageBtn[i].SetActive(false);
                continue;

            }
            string stagetext = (i + page * 8).ToString();
            Debug.Log(("completeStage[" + stagetext + "]"));
            if (PlayerPrefs.GetInt("completeStage["+stagetext+"]") == 1)
            {
                Debug.Log("load" + (i + page * 8));
                Button tmpButton = UIStageBtn[i].GetComponent<Button>();
                ColorBlock cb = tmpButton.colors;
                Color newColor = new Color(1,1,1,0.5f);
                cb.normalColor = newColor;
                tmpButton.colors = cb;
            }
            Text btnTextStageBtn = UIStageBtn[i].GetComponentInChildren<Text>();
            btnTextStageBtn.text = "Stage" + (i + 1 + page * 8); 

            UIStageBtn[i].SetActive(true);
        }
        //Debug.Log(gameManager.maxStage);
        //maxStage = gameManager.maxStage;
        if (page == 0)
        {
            UIPageDown.SetActive(false);
            UIPageUp.SetActive(true);
        }
        else if ((float)(maxStage / ((page+1)*8)) < 1f)
        {
            UIPageDown.SetActive(true);
            UIPageUp.SetActive(false);
        }
        else
        {
            UIPageUp.SetActive(true);
            UIPageDown.SetActive(true);
        }
    }
    public void PageNext()
    {
        Debug.Log("click");
        if ((float)((maxStage-page*8)/8) > 0f) { 
            page += 1;
        }
    }
    public void PageBefore()
    {
        if (page == 0)
        {

        }
        else { 
            page -= 1;
        }
    }

    public void goStage()
    {
        string btn_name = EventSystem.current.currentSelectedGameObject.name;
        for (int i = 1; i < UIStageBtn.Length+1  ; i++)
        {
            if (btn_name.Contains(i.ToString()))
            {
                moveStageIndex = page * 8 + i;
                Debug.Log(moveStageIndex);
                //stageSave.stageIndex_scene0 = moveStageIndex;
                StageSaveAndLoad.stageIndex = moveStageIndex;
                Time.timeScale = 1;
                sceneChange.SceneChange1();
                //SceneManager.LoadScene(1);
            }
        }
    }
}
