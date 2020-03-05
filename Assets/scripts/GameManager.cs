using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //game manager : 점수와 스테이지(이동)를 관리
    public int totalPoint;
    public int stagePoint;
    public int maxStage;
    public int healthSize;
    public float jumpTimer;
    public int saveStage;

    //static public int stagePointMgr;
    public int stageIndex;
    public PlayerMove player;

    //stageManager
    //public StageSaveAndLoad stageSaveAndLoad;

    public int health;
    public GameObject[] map;

    public Image[] UIhealth;
    public Text UIStage;
    public Text UIPoint;
    public GameObject UIRestartBtn;
    public GameObject UIClearBtn;
    public GameObject UIRestartThisStageBtn;
    public GameObject UINextStageBtn;


    //Data Save
    public int[] completeStage;
    //public DataController dataController;
    void Awake()
    {
        /*
        int SetWidth = 16;
        int SetHeight = 9;
        bool fullscreen = true;

        Screen.SetResolution(Screen.width, Screen.width *  SetWidth /  SetHeight,  fullscreen);

    */

        maxStage = StageSaveAndLoad.MaxStage;
        healthSize = 5;
        jumpTimer = 0f;
        if (StageSaveAndLoad.thisStage == 1)
        {
            stageIndex = StageSaveAndLoad.saveStage;
            Debug.Log("restart" + (stageIndex));
            StageSaveAndLoad.thisStage = 0;
        }
        else
        {
            stageIndex = StageSaveAndLoad.stageIndex - 1;
            StageSaveAndLoad.saveStage = stageIndex;
        }
        
        Debug.Log("gamemgr" + (stageIndex));
        map[stageIndex].SetActive(true);

    }
    void Update()
    {
        
        jumpUping();
        //outline.OnEnable();
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }
    public void Start()
    {
     //   dataController.gameData.maxStage = maxStage;
     //   dataController.SaveGameData();
        healthSize = 5;
        jumpTimer = 0f;
        UIStage.text = "STAGE" + (stageIndex + 1);
    }

    // Start is called before the first frame update
    public void NextStage()
    {
        //change stage
        if (stageIndex < map.Length - 1)
        {
            Text btnTextNextStage = UINextStageBtn.GetComponentInChildren<Text>();
            btnTextNextStage.text = "Next Stage";
            UINextStageBtn.SetActive(true);
        }
        else
        {//game clear
            //Player Control Lock
            Time.timeScale = 0;
            //result UI

            //Restart button UI
            Text btnText = UIClearBtn.GetComponentInChildren<Text>();
            btnText.text = "Game Clear!";
            UIClearBtn.SetActive(true);

        }

        //calculate point
        totalPoint += stagePoint;
        stagePoint = 0;
    }
    public void goNextStage()
    {
        //die effect jump
        //player.rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //StageLevel += 1;
        //SceneManager.LoadScene(StageLevel, LoadSceneMode.Single);
        UINextStageBtn.SetActive(false);
        //Time.timeScale = 1;
        map[stageIndex].SetActive(false);
        stageIndex += 1;
        StageSaveAndLoad.saveStage = stageIndex;
        map[stageIndex].SetActive(true);

        UIStage.text = "STAGE" + (stageIndex + 1);
        playerReposition();
        //SceneManager.LoadScene("Stage2");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Player reposition
            if (health > 1)
            {
                playerReposition();
            }
            //Health down
            HealthDown();
        }
    }
    // Update is called once per frame
    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            Debug.Log(health);
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            //all health UI off
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);

            //Player die effect
            player.OnDie();

            //Result UI
            Debug.Log("죽었습니다.");

            //Retry button UI
            UIRestartBtn.SetActive(true);
            UIRestartThisStageBtn.SetActive(true);
        }
    }

    public void jumpUp()
    {
        jumpTimer = 3;
    }
    public void jumpUping()
    {
        if (jumpTimer != 0)
        {
            player.jumpBallEat = true;
            jumpTimer -= Time.deltaTime;
            Debug.Log("jump!!!");
            player.jumpPower = 20;
            //outline.OnEnable();
            //player.spriteRenderer.color = new Color(0,0,0,0); // 투명화
            if (jumpTimer < 0)
            {
                player.jumpBallEat = false;
                //player.spriteRenderer.color = new Color(1, 1, 1, 1);
                jumpTimer = 0;
                player.jumpPower = player.jumpBasicPower;
                //outline.OnDisable();
            }
        }
    }
    public void AddLife()
    {
        if (health < healthSize)
        {
            UIhealth[health].color = new Color(1, 0, 0, 1);
            health++;
            Debug.Log(health);
        }
        else
        {

        }
    }
    void playerReposition()
    {
        player.transform.position = new Vector3(-1.5f, 6, -1);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
        StageSaveAndLoad.thisStage = 1;
        Debug.Log("restart click");

    }
    public void RestartThisStage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        /* //sprite alpha
         player.spriteRenderer.color = new Color(1, 1, 1, 1);
         //sprite flip y -> false
         player.spriteRenderer.flipY = false;
         //collider enable
         player.capsuleCollider.enabled = true;

         health = 5;

         map[stageIndex].SetActive(false);
         map[stageIndex].SetActive(true);

         UIRestartBtn.SetActive(false);
         UIRestartThisStageBtn.SetActive(false);
         //map[stageIndex].SetActive(true);
         totalPoint = 0;
         stagePoint = 0;
         for(int i=0; i< healthSize; i++)
         {
             UIhealth[i].color = new Color(1, 0, 0, 1);
         }
         jumpTimer = 0f;
         UIStage.text = "STAGE" + (stageIndex + 1);
         playerReposition();
         */
    }
    public void FinishStage()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("completeStage["+stageIndex+"]", 1);
        Debug.Log("store"+(stageIndex));
    }

}
