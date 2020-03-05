using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        
        if (gameObject.name.Contains("Monster1") || gameObject.name.Contains("MonsterCat1"))
        {
            anim.SetInteger("monsterNum", 1);
            anim.SetInteger("MonsterNum", 1);

        }
        else if (gameObject.name.Contains("Monster2") || gameObject.name.Contains("MonsterCat2"))
        {
            anim.SetInteger("monsterNum", 2);
            anim.SetInteger("MonsterNum", 2);
        }
        else if (gameObject.name.Contains("Monster3"))
        {
            anim.SetInteger("monsterNum", 3);
        }
            Think();

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        

        //platform check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        // 빔을 쏴서 실제로 맞을 경우
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("platform"));
        if (rayHit.collider == null)
            Turn();

    }


    void Think()
    {
        //set next active
        nextMove = Random.Range(-1, 2);

        //sprite animation
        anim.SetInteger("WalkSpeed", nextMove);

        //Flip sprite
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;

        }

        //recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }


    public void OnDamaged()
    {// after monster died
        Debug.Log("die");
        //sprite alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //sprite flip y
        spriteRenderer.flipY = true;

        //collider disable
        capsuleCollider.enabled = false;

        //die effect jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //destroy
        //DeActive();
        Invoke("DeActive", 5);
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
