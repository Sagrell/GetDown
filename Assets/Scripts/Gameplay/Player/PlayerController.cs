﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    public ParticleSystem destroyedVersion;

    //Main camera
    Camera mainCamera;

    float timeForJump;
    float speed;
    bool willDie;
    bool alive;
    bool onGround;
    //Coroutine
    IEnumerator jumping;

    //Positions
    [HideInInspector]
    public Vector3 playerScreenPos;
    Rigidbody rb;
    int currentPosition;
    //Cach
    Transform _RBTransform;
    Vector3 _RBLocalPosition;
    Quaternion _RBLocalRotation;
    Vector3 _RBLocalScale;
    Vector3 startScale;
    //GameState
    GameState GS;
    //PowerUpManager
    PowerUpManager powerUp;
    //Material
    Material cubeMat;
    bool isHit;
    Animator jump;
    void Start()
    {
        currentPosition = 0;
        jump = GetComponent<Animator>();
        speed = GameManager.Instance.playerSpeed;
        rb = GetComponent<Rigidbody>();
        _RBTransform = rb.transform;
        _RBLocalPosition = _RBTransform.localPosition;
        _RBLocalRotation = _RBTransform.localRotation;
        _RBLocalScale = _RBTransform.localScale;
        startScale = _RBTransform.localScale;
        willDie = false;
        alive = true;
        onGround = false;
        jumping = Jump(Vector3.back * 90, speed, true);
        timeForJump = 1.5f / speed;
        mainCamera = Camera.main;

        isHit = false;
        playerScreenPos = mainCamera.WorldToScreenPoint(_RBTransform.position);

        powerUp = PowerUpManager.Instance;
        GetComponent<MeshFilter>().mesh = SkinManager.cubeMesh;
        cubeMat = SkinManager.cubeMat;
        GetComponent<Renderer>().material = cubeMat;
        destroyedVersion.GetComponent<ParticleSystemRenderer>().sharedMaterial = cubeMat;
        GetComponent<Animator>().enabled = false;
        StartCoroutine("ControllSpeed");
    }

    IEnumerator ControllSpeed()
    {
        while(speed < GameManager.Instance.maxPlayerSpeed)
        {
            speed = GameManager.Instance.playerSpeed * GameState.currentSpeedFactor;
            yield return new WaitForSeconds(.2f);
        }
        speed = GameManager.Instance.maxPlayerSpeed;
    }
    private void FixedUpdate()
    {
        if (!isHit)
        {
            _RBTransform.localRotation = _RBLocalRotation;
            _RBTransform.localPosition = _RBLocalPosition;
            _RBTransform.localScale = _RBLocalScale;
        }
    }
    void Update()
    { 
        //Check for bounds and destroy player if he's out of them
        playerScreenPos = mainCamera.WorldToScreenPoint(_RBTransform.position);
        if ((playerScreenPos.y >= Screen.height || playerScreenPos.y <= 0))
        {
            DestroyPlayer(false);
        }

        if (Input.GetKey("d") && onGround && GameState.playerPositionX < 5)
        {
            JumpRight();
        }
        if (Input.GetKey("a") && onGround && GameState.playerPositionX > 0)
        {
            JumpLeft();
        }
        Touch[] touches = Input.touches;
        if (touches.Length == 1 && onGround)
        {
            Touch touch = touches[0];
            if (touch.position.y <= Screen.height / 3)
            {
                if (touch.position.x > Screen.width / 2 && GameState.playerPositionX < 5)
                {
                    JumpRight();
                }
                else if (touch.position.x < Screen.width / 2 && GameState.playerPositionX > 0)
                {
                    JumpLeft();
                }
            }

        }
    }


    //Jump on the next platform on the right side
    void JumpRight()
    {
        ++currentPosition;
        StopCoroutine(jumping);
        jumping = Jump(Vector3.back * 90, speed, true);        
        StartCoroutine(jumping);
    }
    //Jump on the next platform on the left side
    void JumpLeft()
    {
        --currentPosition;
        StopCoroutine(jumping);
        jumping = Jump(Vector3.forward * 90, -speed, false);
        StartCoroutine(jumping);
    }
    IEnumerator Jump(Vector3 byAngles, float speed, bool dir)
    {
        onGround = false;
        GameState.playerPositionY++;
        GameState.playerPositionX += dir ? 1 : -1;
        timeForJump = Mathf.Abs(1.5f / speed);
        Vector3 prevPosition = _RBLocalPosition;
        float x = 0;
        float y = 0;
        Quaternion rotation = Quaternion.identity;
        Quaternion fromAngle = _RBTransform.localRotation;
        Quaternion toAngle = Quaternion.Euler(fromAngle.eulerAngles + byAngles);
        float height = 0.8f;
        float width = 0.6f;
        Vector3 readyToJumpScale = new Vector3(height, width, height);
        Vector3 jumpScale = new Vector3(width, height, width);
        if (currentPosition%2!=0)
        {
            readyToJumpScale = new Vector3(width, height, height);
            jumpScale = new Vector3(height, width, width);
        }
        for (float t = 0f; true; t += GameManager.fixedDeltaTime / timeForJump)
        {
            //If player is grounded, then stop falling
            if (onGround)
            {
                break;
            }
            x += speed*GameManager.fixedDeltaTime;
            y = Parabolic(Mathf.Abs(x));
            if(t>1)
            {
                t--;
                fromAngle = toAngle;
                toAngle = Quaternion.Euler(fromAngle.eulerAngles + byAngles);
            }
            rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            if (Mathf.Abs(x) > 2f)
            {
                willDie = true;
            }
            _RBLocalPosition.x = prevPosition.x + x;
            _RBLocalPosition.y = prevPosition.y + y;
            _RBLocalRotation = rotation;
            if (t > 0.5f)
            {
                _RBLocalScale = Vector3.Lerp(jumpScale, startScale, (t - 0.5f) * 2f);
            }
            else if (t > 0.1f)
            {
                _RBLocalScale = Vector3.Lerp(readyToJumpScale, jumpScale, (t - 0.1f) * 2.5f);
            }
            else
            {
                _RBLocalScale = Vector3.Lerp(startScale, readyToJumpScale, t * 10);
            }
            yield return new WaitForFixedUpdate();     
        }
    }
    public void DestroyPlayer(bool isMario)
    {
        alive = false;
        GameState.isAlive = false;
        if(!GameState.isLearning)
        {
            GameManager.Instance.Kill();
        } else
        {
            InGameGUI.Instance.DieInLearning(isMario);
        }
        powerUp.DeactivateAll();
        Instantiate(destroyedVersion.gameObject, _RBTransform.position, _RBTransform.rotation, transform.parent).GetComponent<ParticleSystem>().Play();
        AudioCenter.Instance.PlaySound("DestroyPlayer");
        ShakeManager.ShakeAfterDeath();
        StopCoroutine("ControllSpeed");
        gameObject.SetActive(false);
    }
    public void RespawnPlayer()
    {
        currentPosition = 0;
        willDie = false;
        isHit = false;
        _RBLocalRotation = Quaternion.identity;
        _RBLocalPosition.x = (GameState.playerPositionX * 1.5f) - 3.75f;
        _RBLocalPosition.y = GameState.playerPositionY * -1.5f + 0.5f;
        _RBTransform.localRotation = _RBLocalRotation;
        _RBTransform.localPosition = _RBLocalPosition;
        StartCoroutine("ControllSpeed");
    }
    public bool IsAlive()
    {
        return alive;
    }

    int score = 0;
    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if(collider.tag == "Enemy")
        {
           if(!PowerUpManager.isShield)
           {

               if(GameState.isLearning && collider.GetComponent<Diamond>()!=null)
               {
                   DestroyPlayer(true);
               } else
               { 
                   DestroyPlayer(false);
               }
              
           }
                
        }
        if (collider.tag == "Platform")
        {
            if (willDie)
            {  
               DestroyPlayer(false);
            } else
            {
                //Correct position
                _RBLocalPosition.x = collider.transform.localPosition.x;
                _RBLocalPosition.y = collider.transform.localPosition.y + 0.5f;
                _RBLocalScale = startScale;
                _RBLocalRotation = Quaternion.Euler(new Vector3(0,0,currentPosition*90));
                if (!GameState.isLearning)
                {
                    score = ++GameState.score;
                } else
                {
                    score = ++GameState.learningProgress;
                }
                
                switch(score)
                {
                    case 50:
                        GooglePlayManager.Instance.Achieve(GooglePlayManager.firstJumpsAchieve);
                        break;
                    case 150:
                        GooglePlayManager.Instance.Achieve(GooglePlayManager.ugbprofessionalAchieve);
                        break;
                    case 250:
                        GooglePlayManager.Instance.Achieve(GooglePlayManager.murProfessionalAchieve);
                        break;
                    case 500:
                        GooglePlayManager.Instance.Achieve(GooglePlayManager.urdProfessionalAchieve);
                        break;
                    case 1000:
                        GooglePlayManager.Instance.Achieve(GooglePlayManager.impossibleAchieve);
                        break;
                }
                InGameGUI.Instance.UpdateScore();
                onGround = true;
            }
            
        }
        if(collider.tag == "BrokenPlatform")
        {
            isHit = true;
            willDie = true;
            collider.gameObject.GetComponent<BrokenPlatform>().destroyPlatform();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            GameState.countCoins += coin.currentValue;
            other.GetComponent<Coin>().DestroyCoin();
            InGameGUI.Instance.UpdateCoins();
        }
        if (other.CompareTag("ShieldCoin"))
        {
            other.GetComponent<ShieldPowerUp>().DestroyPowerUp();
            powerUp.Activate("Shield");
        }
        if (other.CompareTag("MagnetCoin"))
        {
            other.GetComponent<MagnetPowerUp>().DestroyPowerUp();
            powerUp.Activate("Magnet");
        }
        if (other.CompareTag("DoubleCoinPowerUp"))
        {
            other.GetComponent<DoubleCoinPowerUp>().DestroyPowerUp();
            powerUp.Activate("DoubleCoins");
        }
        if (other.CompareTag("FastRunPowerUp"))
        {
            other.GetComponent<FastRunPowerUp>().DestroyPowerUp();
            powerUp.Activate("FastRun");
        }
    }

    float Parabolic(float x)
    {
        return -2 * x * x + 2 * x;
    }

    /*Vector3 getVelocityByEndAndHeight(Vector3 end, Vector3 height, out float t)
    {
        float x1 = height.x;
        float y1 = height.y;
        float x2 = end.x;
        float y2 = end.y;
        float g = -Physics.gravity.y;

        float t2 = Mathf.Sqrt(2 * Mathf.Abs(y2 - y1) / g);
        float t1 = x1 * t2 / (x2 - x1);
        float alpha = Mathf.Atan((y1 + (g * t1 * t1) / 2) / x1);
        float velocity = x1 / (t1 * Mathf.Cos(alpha));

        t = t1 + t2;
        return new Vector3(velocity * Mathf.Cos(alpha), velocity * Mathf.Sin(alpha));
    }*/

}
