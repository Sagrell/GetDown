using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    public GameObject[] destroyedVersions;

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
    //Cach
    Transform _RBTransform;
    Vector3 _RBLocalPosition;
    Quaternion _RBLocalRotation;

    //GameState
    GameState GS;
    //PowerUpManager
    PowerUpManager powerUp;
    //Material
    Material cubeMat;

    bool isHit;
    GameObject DP;
    void Start()
    {
        speed = GameManager.Instance.playerSpeed;
        rb = GetComponent<Rigidbody>();
        _RBTransform = rb.transform;
        _RBLocalPosition = _RBTransform.localPosition;
        _RBLocalRotation = _RBTransform.localRotation;
        willDie = false;
        alive = true;
        onGround = false;
        jumping = Jump(Vector3.back * 90, speed, true);
        timeForJump = 1.5f / speed;

        mainCamera = Camera.main;

        isHit = false;
        playerScreenPos = mainCamera.WorldToScreenPoint(_RBTransform.position);

        powerUp = PowerUpManager.Instance;

        cubeMat = SkinManager.cubeMat;
        GetComponent<Renderer>().material = cubeMat;

        for (int i = 0; i < destroyedVersions.Length; i++)
        {
            Renderer[] cubeParts = destroyedVersions[i].GetComponentsInChildren<Renderer>();
            for (int j = 0; j < cubeParts.Length; ++j)
            {
                cubeParts[j].material = cubeMat;
            }
        }
        GetComponent<Animator>().enabled = false;
        StartCoroutine(ControllSpeed());
    }
    private void FixedUpdate()
    {
        //Set position and rotation in fixed update cause its depends on collision!
        
    }
    IEnumerator ControllSpeed()
    {
        while(speed < GameManager.Instance.maxPlayerSpeed)
        {
            speed = GameManager.Instance.playerSpeed * GameState.currentSpeedFactor;
            yield return new WaitForSeconds(.1f);
        }   
    }
    void Update()
    {
        if (!isHit)
        {
            _RBTransform.localRotation = _RBLocalRotation;
            _RBTransform.localPosition = _RBLocalPosition;
        }
        //Check for bounds and destroy player if he's out of them
        playerScreenPos = mainCamera.WorldToScreenPoint(_RBTransform.position);
        if ((playerScreenPos.y >= Screen.height || playerScreenPos.y <= 0))
        {
            DestroyPlayer();
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
        StopCoroutine(jumping);
        jumping = Jump(Vector3.back * 90, speed, true);        
        StartCoroutine(jumping);
    }
    //Jump on the next platform on the left side
    void JumpLeft()
    {
       
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
        bool isContinue = false;
        Quaternion fromAngle = _RBTransform.localRotation;
        Quaternion toAngle = Quaternion.Euler(fromAngle.eulerAngles + byAngles);
        for (float t = 0f; true; t += Time.deltaTime/timeForJump)
        {
            //If player is grounded, then stop falling
            if (onGround)
            {
                break;
            }
            x += speed*Time.deltaTime;
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
            //Freeze player on the end position to avoid "miss-position" and check for the collision 
            if (Mathf.Abs(x) > 1.5f && !isContinue)
            {
                x = dir ? 1.5f : -1.5f;
                y = -1.5f;
                rotation = toAngle;
                _RBLocalPosition.x = prevPosition.x + x;
                _RBLocalPosition.y = prevPosition.y + y;
                _RBLocalRotation = rotation;
                _RBTransform.localRotation = _RBLocalRotation;
                _RBTransform.localPosition = _RBLocalPosition;
                yield return new WaitForFixedUpdate();
                if (!onGround)
                {
                    isContinue = true;
                }
            } else
            {
                _RBLocalPosition.x = prevPosition.x + x;
                _RBLocalPosition.y = prevPosition.y + y;
                _RBLocalRotation = rotation;
                //We need to sync it with Physics
                yield return null;
            }      
        }
    }
    public void DestroyPlayer()
    {
        alive = false;
        GameState.isAlive = false;
        GameManager.Instance.Kill();
        powerUp.DeactivateAll();
        DP = Instantiate(destroyedVersions[Random.Range(0,3)], _RBTransform.position, _RBTransform.rotation);
        AudioCenter.Instance.PlaySound("DestroyPlayer");
        ShakeManager.ShakeAfterDeath();
        gameObject.SetActive(false);
    }
    public void RespawnPlayer()
    {
        Destroy(DP);
        willDie = false;
        isHit = false;
        _RBLocalRotation = Quaternion.identity;
        _RBLocalPosition.x = (GameState.playerPositionX * 1.5f) - 3.75f;
        _RBLocalPosition.y = GameState.playerPositionY * -1.5f + 0.5f;
        _RBTransform.localRotation = _RBLocalRotation;
        _RBTransform.localPosition = _RBLocalPosition;
        gameObject.SetActive(true);
    }
    public bool isAlive()
    {
        return alive;
    }


    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if(collider.tag == "Enemy")
        {
           if(!PowerUpManager.isShield)
             DestroyPlayer();    
        }
        if (collider.tag == "Platform")
        {
            if (willDie)
            {  
               DestroyPlayer();
            } else
            {
                
                //AudioCenter.PlaySound(AudioCenter.jumpSoundId);
                //Correct position if somethink happend 
                _RBLocalPosition.x = collider.transform.localPosition.x;
                _RBLocalPosition.y = collider.transform.localPosition.y + 0.5f;
                GameState.score++;
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
            other.GetComponent<Coin>().DestroyCoin();
            GameState.countCoins++;
            InGameGUI.Instance.UpdateCoins();
        }
        if (other.CompareTag("DoubleCoin"))
        {
            other.GetComponent<DoubleCoin>().DestroyCoin();
            GameState.countCoins+=2;
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
