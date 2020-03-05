using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    //Item plus 

    public string type;
    public float value = 0;
    public bool jumpBallEat = false;
    public bool jumpPlusEat = false;
    public bool side = false;
    public int jumpBasicPower = 15;

    //Game manager
    public GameManager gameManager;
    public GameManager uiButtonManager;

    //Data Save
    //public DataController dataController;
    public int completeStage;

    //player move
    public float maxSpeed;
    public float jumpPower;
    public float Ismoving;

    public bool inputLeft = false;
    public bool inputRight = false;
    public bool inputJump = false;
    public bool canJump = true;
    public int jumpNum;
    public bool grounded = false;



    Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    Animator anim;
    public CapsuleCollider2D capsuleCollider;

    //audio
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    AudioSource audioSource;


    void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        //초기화
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    public void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }


    void Update()
    {


        // player moving with button
        if (!inputRight && !inputLeft)
        {
            Ismoving = 0f;
            //Debug.Log("stop");
            anim.SetBool("IsWalking", false);
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.0001f, rigid.velocity.y);

        }
        else if (inputLeft)
        {
            Ismoving = -1f;

            anim.SetBool("IsWalking", true);
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
            //transform.localScale = new Vector3(-1, 1,1);
            spriteRenderer.flipX = true;
        }
        else if (inputRight)
        {
            Ismoving = 1f;

            anim.SetBool("IsWalking", true);
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
            //transform.localScale = new Vector3(1, 1,1);
            spriteRenderer.flipX = false;
        }
        //jumping
        if (inputJump)
        {
            if (side==false)
            {
                inputJump = false;
                if (jumpBallEat == false && jumpPlusEat == false)
                {
                    jumpPlusEat = false;
                    jumpPower = jumpBasicPower;
                }
                if (grounded)
                {
                    grounded = false;
                    jumpPlusEat = false;
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    anim.SetBool("IsJumping", true);
                    PlaySound("JUMP");

                }
            }
        }

    }

    void FixedUpdate()
    {

        //Move by key control, move speed
        float h = Ismoving;
        side = false;
        //default = 50 -> 1초에 50회실행 : 가속도가 붙는다
        rigid.AddForce(Vector2.right * h * 3, ForceMode2D.Impulse);

        //rigid.velocity = player의 현재 속도, vector값
        if (rigid.velocity.x > maxSpeed)
        {
            // 두번째 parameter 는 y축의 값
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            //가속도에 제한을 준다. max값
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
        // platform의 옆으로 맞을 때
        // debug이므로 실제창에서는 보이지 않는다.

        /*
        //platform check
        Vector2 UpRightVec = new Vector2(rigid.position.x, rigid.position.y + 0.3f);
        Vector2 DownRightVec = new Vector2(rigid.position.x, rigid.position.y - 0.3f);
        Vector2 UpLeftVec = new Vector2(rigid.position.x, rigid.position.y + 0.3f);
        Vector2 DownLeftVec = new Vector2(rigid.position.x, rigid.position.y - 0.3f);

        Debug.DrawRay(UpRightVec, Vector3.right, new Color(0, 0, 1));
        Debug.DrawRay(DownRightVec, Vector3.right, new Color(0, 0, 1));
        Debug.DrawRay(UpLeftVec, Vector3.left, new Color(0, 0, 1));
        Debug.DrawRay(DownLeftVec, Vector3.left, new Color(0, 0, 1));
        Debug.DrawRay(rigid.position, Vector3.right, new Color(0, 0, 1));
        Debug.DrawRay(rigid.position, Vector3.left, new Color(0, 0, 1));

        // 빔을 쏴서 실제로 맞을 경우
        RaycastHit2D rayHit_uprightside = Physics2D.Raycast(UpRightVec, Vector3.right, 1, LayerMask.GetMask("platform"));
        RaycastHit2D rayHit_upleftside = Physics2D.Raycast(DownRightVec, Vector3.left, 1, LayerMask.GetMask("platform"));
        RaycastHit2D rayHit_downrightside = Physics2D.Raycast(UpLeftVec, Vector3.right, 1, LayerMask.GetMask("platform"));
        RaycastHit2D rayHit_downleftside = Physics2D.Raycast(DownLeftVec, Vector3.left, 1, LayerMask.GetMask("platform"));
        RaycastHit2D rayHit_right = Physics2D.Raycast(rigid.position, Vector3.right, 1, LayerMask.GetMask("platform"));
        RaycastHit2D rayHit_left = Physics2D.Raycast(rigid.position, Vector3.left, 1, LayerMask.GetMask("platform"));
        if (rayHit_right.collider != null || rayHit_left.collider != null || 
            rayHit_uprightside.collider != null || rayHit_upleftside.collider != null || 
            rayHit_downrightside.collider != null || rayHit_downleftside.collider != null)
        {
            
                if (rayHit_right.distance < 0.5f || rayHit_left.distance < 0.5f 
                    || rayHit_uprightside.distance < 0.5f || rayHit_upleftside.distance < 0.5f 
                    || rayHit_downrightside.distance < 0.5f || rayHit_downleftside.distance < 0.5f)
                {//실제로 벽면에 닿았다.
                    Debug.Log("put");
                    side = true;
                    inputJump = false;
                    jumpPlusEat = false;
                    anim.SetBool("IsJumping", false);
                    jumpNum = 0;
                }
                else
                {
                    side = false;
                }
            
        }
        */

        // landing platform
        if (rigid.velocity.y < 0)
        {
            // debug이므로 실제창에서는 보이지 않는다.
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            // 빔을 쏴서 실제로 맞을 경우
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {//실제로 바닥에 닿았을 때
                    jumpPlusEat = false;
                    anim.SetBool("IsJumping", false);
                    jumpNum = 0;
                }
            }

            Debug.DrawRay(rigid.position, Vector3.down, new Color(1, 0, 0));

            // 빔을 쏴서 실제로 맞을 경우
            RaycastHit2D rayHit2 = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("itemPlusJumpAdd"));
            if (rayHit2.collider != null)
            {
                if (rayHit2.distance < 0.5f)
                {//실제로 jumpitem에 닿았을 때
                    jumpPower = 20;
                    jumpPlusEat = true;
                    anim.SetBool("IsJumping", false);
                    jumpNum = 0;
                }
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        ///*
        ///유니티에서 두개이상의 게임오브젝트가 충돌할때는 
        ///물리적요소를 추가해야하는데 
        ///이때 추가요소를 Collider(콜라이더)라고 합니다.
        /// */
        /// 
        

        if (collision.transform.name == "Platform" || 
            collision.transform.name == "itemJump" || 
            collision.transform.name == "itemMove")
        {
            grounded = true;
        }


        if (collision.gameObject.tag == "monster")
        {
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                //attack
                OnAttack(collision.transform);
            }
            else
            { //damaged
                OnDamaged(collision.transform.position);
            }
        }
        if (collision.gameObject.tag == "hurdle")
        {
            OnDamaged(collision.transform.position);

        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "life")
        {
            gameManager.AddLife();
            PlaySound("ITEM");
            collision.gameObject.SetActive(false);

        }
        if (collision.gameObject.tag == "jumpBall")
        {
            gameManager.jumpUp();
            PlaySound("ITEM");
            collision.gameObject.SetActive(false);

        }
        if (collision.gameObject.tag == "item")
        {

            bool isBronze = collision.gameObject.name.Contains("BronzeFish");
            bool isSilver = collision.gameObject.name.Contains("SilverFish");
            bool isGold = collision.gameObject.name.Contains("GoldFish");

            PlaySound("ITEM");

            //Point : 점수 얻기
            if (isBronze)
            {
                gameManager.stagePoint += 10;
            }
            else if (isSilver)
            {
                gameManager.stagePoint += 50;
            }
            else if (isGold)
            {
                gameManager.stagePoint += 100;
            }


            //Deactive Item
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            PlaySound("FINISH");

            //dataController.gameData.completeStage[gameManager.stageIndex] = 1;
            //dataController.SaveGameData();
            //Next Stage : game manager가 하는 일
            gameManager.FinishStage();
            gameManager.NextStage();
        }
    }




    void OnAttack(Transform monster)
    {
        PlaySound("ATTACK");
        //Point
        gameManager.stagePoint += 200;

        // reaction force
        // 몬스터 밟았을때 player도 뛴다.
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Monster die
        MonsterMove monsterMove = monster.GetComponent<MonsterMove>();
        monsterMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos)
    {
        jumpNum = 1;
        PlaySound("DAMAGED");
        //health down
        gameManager.HealthDown();

        // change layer (Immortal active)
        gameObject.layer = 11; // playerdamaged layer가 11번

        // view alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // reaction force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 8, ForceMode2D.Impulse);

        //Animation
        anim.SetTrigger("IsDamaged");

        Invoke("OffDamaged", 1.5f);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        PlaySound("DIE");

        //sprite alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //sprite flip y
        spriteRenderer.flipY = true;

        //collider disable
        capsuleCollider.enabled = false;

        //die effect jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }






}
