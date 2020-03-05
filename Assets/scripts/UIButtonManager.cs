using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonManager : MonoBehaviour
{
   
    GameObject player;
    PlayerMove playerScript;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMove>();
    }

    public void LeftDown()
    {
        playerScript.inputLeft = true;
        Debug.Log("left down");
    }
    public void LeftUp()
    {
        playerScript.inputLeft = false;
        Debug.Log("left up");
    }
    public void RightDown()
    {
        playerScript.inputRight = true;
        Debug.Log("right down");

    }
    public void RightUp()
    {
        playerScript.inputRight = false;
        Debug.Log("right up");
    }
    public void JumpClick()
    {
        playerScript.inputJump = true;
        Debug.Log("jump ");
    }
}
