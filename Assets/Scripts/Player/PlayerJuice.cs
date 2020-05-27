using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class PlayerJuice : MonoBehaviour
{

    SoundController m_sounds;
    Rigidbody2D m_rb;
    PlayerController m_controller;
    Camera cam;

    bool scalingOnVel = true;

    float scaleModx = 0;
    float scaleMody = 0;


    [Header("Velocity Scale Settings")]
    [SerializeField] float scaleStrenght = 0.1f;


    [Header("Wall Bump Settings")]
    [SerializeField] Transform m_SpriteObject = default;
    [SerializeField] [Range(0.2f, 1f)] float scaleAmount = 0.1f;
    [SerializeField] [Range(0.1f, 1f)] float bopDuration = 0.1f;

    [Header("Screen shakes")]
    [SerializeField] float bumpShakeMultiplier = 0.15f;
    [SerializeField] float deathScreenShakeStr = 2f;
    Vector3 oldScale = Vector3.one;
    private void Start()
    {
        m_sounds = GetComponentInChildren<SoundController>();
        m_rb = GetComponent<Rigidbody2D>();
        m_controller = GetComponent<PlayerController>();
        cam = Camera.main;

        if (m_SpriteObject == null)
            Debug.Log("Assign the transform of players sprite on " + gameObject.name);

    }


    private void OnCollisionEnter2D(Collision2D other) //Juice on collision with other magnets
    {

        ScreenShake(other);

        PlayerBop(other);


        void ScreenShake(Collision2D col)
        {
            Vector2 directionFromPlayer = col.transform.position - transform.position;
            Tween cameraBump = cam.DOShakeRotation(0.15f, directionFromPlayer * 0.15f, 3, 5f, true);

            if (cam.transform.rotation.eulerAngles == Vector3.zero)
                cameraBump.Play();
            else
                cam.transform.DORotate(Vector3.zero, 0.1f);
        }
        void PlayerBop(Collision2D col)
        {
            float myY = transform.position.y;
            float otherY = col.transform.position.y;

            if (scalingOnVel && myY != otherY && col.transform.CompareTag("Positive") || scalingOnVel && myY != otherY && col.transform.CompareTag("Negative"))
            {
                //TODO
                //            m_sounds.Play("wallStick");

                scalingOnVel = false;

                Vector2 punchScale = new Vector2(scaleAmount, -scaleAmount);
                oldScale = m_SpriteObject.transform.localScale;
                m_SpriteObject.DOPunchScale(punchScale, bopDuration, 2, 0).SetEase(Ease.OutQuad);

            }
        }
    }

    private void Update()
    {
        ScaleBasedOnVelocity();
        AdjustPositionForSquish();
    }

    private void ScaleBasedOnVelocity()
    {
        //Ta script poskbri, da se 

        //turn velocity scaling back on if needed
        if (!scalingOnVel && m_SpriteObject.transform.localScale == oldScale)
            scalingOnVel = true;

        scaleModx = Mathf.Abs(m_rb.velocity.x) * scaleStrenght;
        scaleMody = Mathf.Abs(m_rb.velocity.y) * scaleStrenght;
        if (scalingOnVel)
        {
            Vector3 scaleVector = new Vector3(1 + scaleModx - scaleMody, 1 - scaleModx + scaleMody, 1);
            //m_SpriteObject.localScale = scaleVector;
            m_SpriteObject.localScale = Vector3.Lerp(m_SpriteObject.localScale, scaleVector, 0.5f);
        }
    }

    void AdjustPositionForSquish()
    {
        //Ta skript poskbri, da je positiononing karakterja nomralen po tem, ko ga ScaleBasedOnVelocity scala
        float yMod = 1 - m_SpriteObject.localScale.y;
        if (m_controller.yForce <= 0 && Mathf.Abs(m_rb.transform.rotation.eulerAngles.z) == 180 || m_controller.yForce > 0 && Mathf.Abs(m_rb.transform.rotation.eulerAngles.z) != 180)
            yMod = -yMod;


        Vector3 adjustedPosition = new Vector3(0, -yMod, 0);
        m_SpriteObject.localPosition = adjustedPosition;
    }

    public void DeathScreenshake()
    {
        cam.DOShakeRotation(0.3f, m_rb.velocity.normalized * deathScreenShakeStr, 5, 10f, true);
    }
}