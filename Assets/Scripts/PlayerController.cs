using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    [Header("Character stats")]
    [SerializeField] bool rayDebugging = true;
    [Space]
    [SerializeField] bool startFlipped = false;
    [SerializeField] bool magnetised = true;
    [SerializeField] float speedMod = 5f;
    //Input values
    float xInput;

    [Header("Magnetism Values")]
    [SerializeField] [Range(1, 5)] float magnetLenght = 5f;
    [SerializeField] float yForce = 0;
    [SerializeField] float yForceClamp = 25f;
    [SerializeField] LayerMask groundLayer = default;
    public bool grounded = false;
    float yPosForce = 0;
    float yNegForce = 0;

    [Header("Object References")]
    [SerializeField] public ParticleSystem dustClouds = default;
    Animator m_animator;
    Rigidbody2D m_rigibody;
    AudioSource blip;
    Camera cam;

    void Start()
    {
        m_rigibody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        blip = GetComponent<AudioSource>();
        cam = Camera.main;
        if (startFlipped)
            m_animator.SetTrigger("Rotate180+");
    }

    void Update()
    {
        xInput = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Debug.Log("Jump axis active");
            blip.Play();

            //TODO menjaj na aniamtion state based switch statment
            /*switch (switch_on)
            {
                
                default:
            }*/

            if (transform.rotation.eulerAngles.z == 0)
                m_animator.SetTrigger("Rotate180+");
            if (transform.rotation.eulerAngles.z == 180)
                m_animator.SetTrigger("Rotate180-");
        }

    }

    private void FixedUpdate()
    {
        RaycastHit2D negativeHit = DrawRay(transform, transform.up, Color.red);
        RaycastHit2D positiveHit = DrawRay(transform, transform.up * -1, Color.blue);

        yForce = YForce(positiveHit, negativeHit);

        float yForceAddition = (Mathf.Abs(yForce) / 2) * xInput;
        Vector2 direction = new Vector2(xInput * speedMod * m_rigibody.mass + yForceAddition, yForce);

        m_rigibody.AddForce(direction);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 directionFromPlayer = other.transform.position - transform.position;

        if (!grounded && cam.transform.rotation == Quaternion.Euler(Vector3.zero))
            cam.DOShakeRotation(0.1f, directionFromPlayer.normalized, 1, 5f, true);

        if (other.gameObject.CompareTag("Positive") || other.gameObject.CompareTag("Negative") || other.gameObject.CompareTag("Neutral"))
            grounded = true;

    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Positive") || other.gameObject.CompareTag("Negative") || other.gameObject.CompareTag("Neutral"))
            grounded = false;
    }

    //Simulacija magnetnih sil
    float YForce(RaycastHit2D positive, RaycastHit2D negative)
    {
        #region  NegativeHit
        if (negative.collider != null && magnetised) //ce hita karkoli gremo racunat sile
        {
            //preveri če je magnet nad mano ali pod mano, če je and mano potem invertiraj sile
            int aboveMod = 1;
            if (negative.collider.gameObject.transform.position.y > transform.position.y)
                aboveMod *= -1;

            if (negative.collider.CompareTag("Positive")) //Privalcita se -> EaseIn
                yNegForce = LibBrin_Lerp.EaseIn(yNegForce, -yForceClamp, 0.5f) * aboveMod;
            //yNegForce = LerpEaseOut(yNegForce, -yForceClamp, 0.8f) * aboveMod;

            else if (negative.collider.CompareTag("Negative")) //Odvracata se -> EaseOut
                yNegForce = LibBrin_Lerp.EaseOut(yNegForce, yForceClamp, 0.5f) * aboveMod;
            //yNegForce = LerpEaseOut(yNegForce, yForceClamp, 0.8f) * aboveMod;
        }
        else
            yNegForce = Mathf.Lerp(yNegForce, 0, 0.5f);
        #endregion

        #region  PositiveHit
        if (positive.collider != null && magnetised)
        {
            int aboveMod = 1;
            if (positive.collider.gameObject.transform.position.y > transform.position.y)
                aboveMod *= -1;

            if (positive.collider.CompareTag("Negative")) //Privlacita se -> EaseIn
                yPosForce = LibBrin_Lerp.EaseIn(yPosForce, -yForceClamp, 0.5f) * aboveMod;
            //    yPosForce = LerpEaseOut(yPosForce, -yForceClamp, 0.8f) * aboveMod;

            else if (positive.collider.CompareTag("Positive")) //odvracata se -> EaseOut
                yPosForce = LibBrin_Lerp.EaseOut(yPosForce, yForceClamp, 0.5f) * aboveMod;
            //    yPosForce = LerpEaseOut(yPosForce, yForceClamp, 0.8f) * aboveMod;
        }
        else
            yPosForce = Mathf.Lerp(yPosForce, 0, 0.5f);
        #endregion

        return yPosForce + yNegForce;
    }

    public RaycastHit2D DrawRay(Transform myTrans, Vector2 direction, Color debugColor)
    {
        if (rayDebugging)
            Debug.DrawRay(myTrans.position, direction * magnetLenght, debugColor);
        return Physics2D.Raycast(myTrans.position, direction, magnetLenght, groundLayer);
    }

    float LerpEaseOut(float a, float b, float t)
    {
        //t = Mathf.Sin(t * Mathf.PI * 0.5f);
        return Mathf.Lerp(a, b, t);
    }

}