using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField] bool rayDebugging = true;
    [SerializeField] SpriteRenderer debugIndicator = null;
    [Space]

    //Input values
    float xInput;



    [Header("Character stats")]
    public bool startFlipped = false;
    [SerializeField] bool magnetised = false;
    [SerializeField] float speedMod = 5f;
    [SerializeField] [Range(0.01f, 0.5f)] float minMangetForce = 0.1f;
    float originalGravityScale;

    //Rotation lerp values
    [SerializeField] [Range(0.1f, 0.5f)] float rotationTime = 0.5f;
    float currentRotationTime;
    float rotateStart;
    float rotateTarget;

    bool interpolateFrom0 = false;
    //MAGNETISM CONFIGURATOR OVER


    [Space]
    [Header("Magnetism Values")]
    [SerializeField] [Range(1, 5)] float magnetLenght = 5f;
    public float yForce = 0;
    [SerializeField] float yForceClamp = 25f;
    [SerializeField] LayerMask groundLayer = default;
    public bool grounded = false;
    float yPosForce = 0;
    float yNegForce = 0;

    [Header("Object References")]
    [SerializeField] public ParticleSystem dustClouds = default;
    Rigidbody2D m_rigibody;
    SoundController m_Sounds = default;



    void Start()
    {
        m_rigibody = GetComponent<Rigidbody2D>();
        m_Sounds = GetComponentInChildren<SoundController>();

        //Original values
        originalGravityScale = m_rigibody.gravityScale;

        //start flipped
        if (startFlipped)
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);

        rotateStart = transform.localRotation.eulerAngles.z;
        rotateTarget = transform.localRotation.eulerAngles.z;
    }

    void Update() //Player controlls
    {
        xInput = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        LerpRotation();
        DebugInterpolation();
        grounded = GroundCheck();

    }


    private void LerpRotation()
    {
        if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump"))
        {

            currentRotationTime = 0f;
            rotateStart = transform.rotation.eulerAngles.z;

            //selection
            switch (transform.rotation.eulerAngles.z)
            {
                case 0:
                    if (xInput == 1)
                        rotateTarget = -180;
                    else
                        rotateTarget = 180;
                    break;

                case 180:
                    if (xInput == 1)
                        rotateTarget = 360;
                    rotateTarget = 0;
                    break;

                default:
                    switch (rotateTarget)
                    {
                        case 180:
                            rotateTarget = 0;
                            break;
                        case 0:
                            rotateTarget = 180;
                            break;
                        default:
                            break;
                    }
                    break;
            }
            //rotateTarget = rotateStart + 180;
        }

        currentRotationTime += Time.deltaTime;
        if (currentRotationTime > rotationTime)
            currentRotationTime = rotationTime;

        float perc = currentRotationTime / rotationTime;

        float newRotation = LibBrin_Lerp.EaseOut(rotateStart, rotateTarget, perc);


        m_rigibody.rotation = newRotation;

        if (transform.rotation.eulerAngles.z % 180 == 0)
            magnetised = true;
        else
            magnetised = false;
    }

    private void FixedUpdate()
    {

        //vertical movement calculation
        RaycastHit2D negativeHit = DrawRay(transform, transform.up, Color.red);
        RaycastHit2D positiveHit = DrawRay(transform, transform.up * -1, Color.blue);

        yForce = YForce(positiveHit, negativeHit);

        float yForceAddition = (Mathf.Abs(yForce) / 2) * xInput;
        float xForceAddition = CheckForWalls();

        Vector2 direction = new Vector2(xForceAddition, yForce);


        m_rigibody.AddForce(direction);

        //Wall checking
        float CheckForWalls()
        {
            #region Raycasting //fireja raycaste, ki preverijo za zid. Ce je zid spremeni xForce v 0.
            Vector3 _ySizeModifier = new Vector3(0, transform.localScale.y * 0.5f, 0) * 0.95f;
            Vector3 _xSizeModifier = new Vector3(xInput * (transform.localScale.x / 2 + 0.01f), 0, 0);
            Color xRayDebugColor = Color.yellow;

            if (rayDebugging)
            {
                Debug.DrawRay(transform.position + _ySizeModifier, _xSizeModifier, xRayDebugColor); //Top Ray
                Debug.DrawRay(transform.position, _xSizeModifier, xRayDebugColor); //Mid Ray
                Debug.DrawRay(transform.position - _ySizeModifier, _xSizeModifier, xRayDebugColor); //Top Ray
            }

            //tripleRay
            RaycastHit2D topRay = Physics2D.Raycast(transform.position + _ySizeModifier, new Vector2(xInput, 0), (transform.localScale.x / 2 + 0.005f), groundLayer);
            RaycastHit2D midRay = Physics2D.Raycast(transform.position, new Vector2(xInput, 0), (transform.localScale.x / 2 + 0.005f), groundLayer);
            RaycastHit2D botRay = Physics2D.Raycast(transform.position - _ySizeModifier, new Vector2(xInput, 0), (transform.localScale.x / 2 + 0.005f), groundLayer);
            #endregion 

            if (topRay || midRay || botRay)
                xForceAddition = 0;
            else
                return xInput * speedMod * m_rigibody.mass + yForceAddition;
            return xForceAddition;

        }
    }

    //Simulacija magnetnih sil
    float YForce(RaycastHit2D positive, RaycastHit2D negative)
    {

        #region  NegativeHit
        if (negative.collider != null && magnetised) //ce hita karkoli gremo racunat sile
        {

            float a = yNegForce;
            if (interpolateFrom0)
                a = 0;

            float distance = negative.distance;
            float distancePerc = 1f - (distance / magnetLenght) + minMangetForce; //returna 0 če je range na max in je na 1 ko smo prakticno zravaven hita 

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
            float distancePerc = 1f - (distance / magnetLenght) + minMangetForce; //returna 0 če je range na max in je na 1 ko smo prakticno zravaven hita 


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


        float sum = yPosForce + yNegForce;

        if (sum <= 0.01f && sum >= -0.01f) //korekcija gravitacije, ko ni magnetnih sil
        {
            Debug.Log("Popravljam na 0");
            if (yPosForce != 0 || yNegForce != 0) //ce smo v freefallu kjer ni magnetnih sil povečamo gravitacijo za to, da ne padamo sto let
                m_rigibody.gravityScale = 1;

            return 0;
        }
        else
        {
            m_rigibody.gravityScale = originalGravityScale;
            return yPosForce + yNegForce;
        }



    }

    public RaycastHit2D DrawRay(Transform myTrans, Vector2 direction, Color debugColor)
    {
        if (rayDebugging)
            Debug.DrawRay(myTrans.position, direction * magnetLenght, debugColor);
        return Physics2D.Raycast(myTrans.position, direction, magnetLenght, groundLayer);
    }

    public bool GroundCheck()
    {
        Vector3 _xMod = new Vector2((transform.localScale.x * 0.5f) - 0.15f, 0);
        float _rayLenght = transform.localScale.y * 0.5f + 0.2f;

        if (rayDebugging) //debug display of rays
        {
            Color groundCheckColor = Color.green;
            Debug.DrawRay(transform.position - _xMod, Vector3.up * _rayLenght, groundCheckColor);
            Debug.DrawRay(transform.position + _xMod, Vector3.up * _rayLenght, groundCheckColor);
            Debug.DrawRay(transform.position + _xMod, Vector3.up * -_rayLenght, groundCheckColor);
            Debug.DrawRay(transform.position - _xMod, Vector3.up * -_rayLenght, groundCheckColor);

        }



        RaycastHit2D leftRayUp = Physics2D.Raycast(transform.position - _xMod, Vector3.up, _rayLenght, groundLayer);
        RaycastHit2D rightRayUp = Physics2D.Raycast(transform.position + _xMod, Vector3.up, _rayLenght, groundLayer);
        RaycastHit2D leftRayDown = Physics2D.Raycast(transform.position - _xMod, Vector3.up * -1, _rayLenght, groundLayer);
        RaycastHit2D rightRayDown = Physics2D.Raycast(transform.position + _xMod, Vector3.up * -1, _rayLenght, groundLayer);

        if (leftRayDown || leftRayUp || rightRayDown || rightRayUp)
            return true;

        else
            return false;
    }


    //Debug functions
    void DebugInterpolation()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (interpolateFrom0)
            { interpolateFrom0 = false; debugIndicator.gameObject.SetActive(false); }
            else
            { interpolateFrom0 = true; debugIndicator.gameObject.SetActive(true); }
        }
    }
}