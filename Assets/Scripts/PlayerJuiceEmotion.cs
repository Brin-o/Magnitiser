using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJuiceEmotion : MonoBehaviour
{
    [SerializeField] Transform mouth = null;
    Vector3 orgMouthScale;
    Transform playerT;
    Rigidbody2D playerRB;

    void Start()
    {
        orgMouthScale = mouth.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
        Vector3 playerSpeed = playerRB.velocity * 0.05f;
        transform.position = Vector3.LerpUnclamped(transform.position, playerT.position + playerSpeed + (new Vector3(0, 0.09f)), .5f);


        Vector3 _mouthAdjust = new Vector3(0, Mathf.Abs(playerSpeed.x));
        mouth.localScale = orgMouthScale - _mouthAdjust;
    }

    public void FindPlayer()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1);
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

    }
}
