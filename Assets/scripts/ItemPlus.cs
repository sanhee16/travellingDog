using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlus : MonoBehaviour
{

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    public int nextMove;
    public Vector3 startPos;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        startPos = gameObject.transform.position;
        Debug.Log(startPos);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }


    void FixedUpdate()
    {

        //platform check
        Vector2 frontVec = new Vector2(rigid.position.x + 0.5f, rigid.position.y);
        Vector2 Backvec = new Vector2(rigid.position.x - 0.5f, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        Debug.DrawRay(Backvec, Vector3.down, new Color(0, 1, 0));

        // 빔을 쏴서 실제로 맞을 경우
        RaycastHit2D rayHit1 = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("platform"));
        RaycastHit2D rayHit2 = Physics2D.Raycast(Backvec, Vector3.down, 1, LayerMask.GetMask("platform"));
        if (rayHit1.collider == null || rayHit2.collider == null)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        }
            

    }
}
