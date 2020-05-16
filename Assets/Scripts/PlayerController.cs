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



    //MAGNETISM CONFIGURATOR
    //Spreminjanje teh vrednosti bo se igralo s tem kako delujejo 
    enum MagnetCalcType
    {
        timeBased,
        distanceBased
    }
    [Header("Interpolation rules")]
    MagnetCalcType calcType = MagnetCalcType.distanceBased;
    bool interpolateFrom0 = false;
    //MAGNETISM CONFIGURATOR OVER


    [SerializeField] [Range(0.01f, 0.5f)] float aLerpMin = 0.1f;

    [Space]

    [Header("Magnetism Values")]
    [SerializeField] [Range(1, 5)] float magnetLenght = 5f;
    [SerializeField] float yForce = 0;
    [SerializeField] float yForceClamp = 25f;
    [SerializeField] LayerMask groundLayer = default;
    public bool grounded = false;
    float yPosForce = 0;
    float yNegForce = 0;
    Vector2 prevVel;

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

        prevVel = m_rigibody.velocity;
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

        AddHitFroce(prevVel);
    }
    void AddHitFroce(Vector2 vel)
    {
        int mod = 1;
        if (vel.y < 0)
            mod *= -1;

        float addingVel = Mathf.Abs(vel.x) * mod;
        addingVel *= 25f;

        m_rigibody.AddForce(new Vector2(0, addingVel));
    }



    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Positive") || other.gameObject.CompareTag("Negative") || other.gameObject.CompareTag("Neutral"))
            grounded = false;
    }

    //Simulacija magnetnih sil
    float YForce(RaycastHit2D positive, RaycastHit2D negative)
    {

        switch (calcType)
        {


            case MagnetCalcType.distanceBased:
                #region  NegativeHit
                if (negative.collider != null && magnetised) //ce hita karkoli gremo racunat sile
                {

                    float a = yNegForce;
                    if (interpolateFrom0)
                        a = 0;

                    float distance = negative.distance;
                    float distancePerc = 1f - (distance / magnetLenght) + aLerpMin; //returna 0 če je range na max in je na 1 ko smo prakticno zravaven hita 

                    //preveri če je magnet nad mano ali pod mano, če je and mano potem invertiraj sile
                    int aboveMod = 1;
                    if (negative.collider.gameObject.transform.position.y > transform.position.y)
                        aboveMod *= -1;

                    if (negative.collider.CompareTag("Positive")) //Privalcita se -> EaseIn
                        yNegForce = LibBrin_Lerp.EaseIn(a, -yForceClamp, distancePerc) * aboveMod;


                    else if (negative.collider.CompareTag("Negative")) //Odvracata se -> EaseOut
                        yNegForce = LibBrin_Lerp.EaseOut(a, yForceClamp, distancePerc) * aboveMod;

                    if (Mathf.Approximately(yNegForce, 0f))
                        yNegForce = 0f;

                }
                else
                    yNegForce = Mathf.Lerp(yNegForce, 0, 0.5f);
                #endregion

                #region  PositiveHit
                if (positive.collider != null && magnetised)
                {

                    float a = yPosForce;
                    if (interpolateFrom0)
                        a = 0;

                    float distance = positive.distance;
                    float distancePerc = 1f - (distance / magnetLenght) + aLerpMin; //returna 0 če je range na max in je na 1 ko smo prakticno zravaven hita 


                    int aboveMod = 1;
                    if (positive.collider.gameObject.transform.position.y > transform.position.y)
                        aboveMod *= -1;

                    if (positive.collider.CompareTag("Negative")) //Privlacita se -> EaseIn
                        yPosForce = LibBrin_Lerp.EaseIn(a, -yForceClamp, distancePerc) * aboveMod;


                    else if (positive.collider.CompareTag("Positive")) //odvracata se -> EaseOut
                        yPosForce = LibBrin_Lerp.EaseOut(a, yForceClamp, distancePerc) * aboveMod;


                    if (Mathf.Approximately(yPosForce, 0f))
                        yPosForce = 0f;
                }
                else
                    yPosForce = Mathf.Lerp(yPosForce, 0, 0.5f);
                #endregion

                return yPosForce + yNegForce; ;

            #region Old-Magnetism
            case MagnetCalcType.timeBased: //TA KODA JE TU SAMO ZA REFERENCO, TRENUTNO SE NE UPORABLJA
                #region  NegativeHit
                if (negative.collider != null && magnetised) //ce hita karkoli gremo racunat sile
                {
                    //preveri če je magnet nad mano ali pod mano, če je and mano potem invertiraj sile
                    int aboveMod = 1;
                    if (negative.collider.gameObject.transform.position.y > transform.position.y)
                        aboveMod *= -1;

                    if (negative.collider.CompareTag("Positive")) //Privalcita se -> EaseIn
                        yNegForce = LibBrin_Lerp.EaseOut(yNegForce, -yForceClamp, 0.5f) * aboveMod;


                    else if (negative.collider.CompareTag("Negative")) //Odvracata se -> EaseOut
                        yNegForce = LibBrin_Lerp.EaseIn(yNegForce, yForceClamp, 0.5f) * aboveMod;

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


                    else if (positive.collider.CompareTag("Positive")) //odvracata se -> EaseOut
                        yPosForce = LibBrin_Lerp.EaseOut(yPosForce, yForceClamp, 0.5f) * aboveMod;

                }
                else
                    yPosForce = Mathf.Lerp(yPosForce, 0, 0.5f);
                #endregion

                return yPosForce + yNegForce;
            #endregion

            default:
                Debug.LogError("This is not a correct enum, this shouldn't happen ever, check code.");
                return default;
        }

    }

    public RaycastHit2D DrawRay(Transform myTrans, Vector2 direction, Color debugColor)
    {
        if (rayDebugging)
            Debug.DrawRay(myTrans.position, direction * magnetLenght, debugColor);
        return Physics2D.Raycast(myTrans.position, direction, magnetLenght, groundLayer);
    }

}