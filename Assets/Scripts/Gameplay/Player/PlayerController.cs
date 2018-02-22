using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public GameObject destroyVersion;
    public GameObject shield;
    public InGameGUI gui;

    [Space]
    [Header("Start speed")]
    public float speed = 3f;

    [Space]
    [Header("Camera shake")]
    public Vector3 positionInfluence;
    public float magnitude;
    public float roughness;
    public float duration;

    //Main camera
    Camera mainCamera;
    CameraShakeInstance _shakeInstance;

    float timeForJump;
    float startSpeed;
    bool willDie;
    bool alive;
    bool onGround;
    bool isShield;
    float countShield;
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

    Vector3 _RBPrevLocalPosition;
    //GameState
    GameState GS;
    //ObjectPooler
    ElementsPool objectPooler;

    //Sounds
    int coinSoundId;
    int jumpSoundId;
    int destroyPlayerSoundId;
    int shieldUpSoundId;
    //Cube Model
    public CubeModel currentCubeModel;
    void Start()
    {
        coinSoundId = AudioCenter.loadSound("CoinCollect");
        jumpSoundId = AudioCenter.loadSound("Jump");
        destroyPlayerSoundId = AudioCenter.loadSound("DestroyPlayer");
        shieldUpSoundId = AudioCenter.loadSound("ShieldUp");
        rb = GetComponent<Rigidbody>();
        _RBTransform = rb.transform;
        _RBLocalPosition = _RBTransform.localPosition;
        _RBLocalRotation = _RBTransform.localRotation;
        _RBPrevLocalPosition = _RBLocalPosition;
        willDie = false;
        alive = true;
        onGround = true;
        jumping = Jump(Vector3.back * 90, speed, true);
        timeForJump = 1.5f / speed;

        mainCamera = Camera.main;
        _shakeInstance = CameraShaker.Instance.StartShake(magnitude, roughness, 0, positionInfluence, new Vector3(0,0,0)); 
        _shakeInstance.StartFadeOut(0);
        _shakeInstance.DeleteOnInactive = false;

        playerScreenPos = mainCamera.WorldToScreenPoint(_RBTransform.position);
        startSpeed = speed;
        objectPooler = ElementsPool.Instance;
        GetComponent<Renderer>().material = currentCubeModel.material;
        foreach(Renderer rend in destroyVersion.GetComponentsInChildren<Renderer>())
        {
            rend.material = currentCubeModel.material;
        }
    }

    private void FixedUpdate()
    {
        speed = startSpeed * GameState.currentSpeedFactor;
        //Set position and rotation in fixed update cause its depends on collision!
        if (!willDie && _RBPrevLocalPosition.y != _RBLocalPosition.y)
        {
            _RBTransform.localRotation = _RBLocalRotation;
            _RBTransform.localPosition = _RBLocalPosition;
            _RBPrevLocalPosition = _RBLocalPosition;
        }
    }
    void Update()
    {
        //Check for bounds and destroy player if he's out of them
        playerScreenPos = mainCamera.WorldToScreenPoint(_RBTransform.position);
        if ((playerScreenPos.y >= Screen.height || playerScreenPos.y <= 0))
        {
            DestroyPlayer();
        }

        if (Input.GetKey("d") && onGround)
        {
            JumpRight();
        }
        if (Input.GetKey("a") && onGround)
        {
            JumpLeft();
        }
        if (Input.GetKeyDown("f"))
        {
            _shakeInstance.Magnitude = magnitude;
            _shakeInstance.Roughness = roughness;
            _shakeInstance.PositionInfluence = positionInfluence;
            _shakeInstance.StartFadeIn(0);
            _shakeInstance.StartFadeOut(duration);
        }
        Touch[] touches = Input.touches;
        if (touches.Length == 1 && onGround)
        {
            Touch touch = touches[0];
            if (touch.position.y <= Screen.height / 3)
            {
                if (touch.position.x > Screen.width / 2)
                {
                    JumpRight();
                }
                else
                {
                    JumpLeft();
                }
            }

        }
    }


    //Jump on the next platform on the right side
    void JumpRight()
    {
        onGround = false;
        StopCoroutine(jumping);
        jumping = Jump(Vector3.back * 90, speed, true);        
        StartCoroutine(jumping);
    }
    //Jump on the next platform on the left side
    void JumpLeft()
    {
        onGround = false;
        StopCoroutine(jumping);
        jumping = Jump(Vector3.forward * 90, -speed, false);
        StartCoroutine(jumping);
    }

    IEnumerator Jump(Vector3 byAngles, float speed, bool dir)
    {
        timeForJump = Mathf.Abs(1.5f / speed);
        Vector3 prevPosition = _RBLocalPosition;
        float x = 0;
        float y = 0;
        Quaternion rotation;
        bool isFirstTime = true;
        Quaternion fromAngle = _RBTransform.localRotation;
        Quaternion toAngle = Quaternion.Euler(fromAngle.eulerAngles + byAngles);
        for (float t = 0f; true; t += GameManager.fixedDeltaTime/timeForJump)
        {
            //If player is grounded, then stop falling
            if (onGround || willDie)
            {
                break;
            }
            x += speed*GameManager.fixedDeltaTime;
            y = parabolic(Mathf.Abs(x));
            rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            if (Mathf.Abs(x) > 2f)
            {
                willDie = true;
            }
            //Freeze player on the end position to avoid "miss-position" and check for the collision 
            if(Mathf.Abs(x) > 1.5f && isFirstTime)
            {
                x = dir ? 1.5f : -1.5f;
                y = -1.5f;
                isFirstTime = false;
                rotation = toAngle;
                t = 0;
            }
            _RBLocalPosition.x = prevPosition.x + x;
            _RBLocalPosition.y = prevPosition.y + y;
            _RBLocalRotation = rotation;
            //We need to sync it with Physics
            yield return new WaitForFixedUpdate();
        }
    }
    public void DestroyPlayer()
    {
        alive = false;
        FindObjectOfType<GameManager>().Kill();
        Shield shield = GetComponentInChildren<Shield>();
        if (shield)
        {
            shield.DestroyShieldimmediately();
        }
        Instantiate(destroyVersion, _RBTransform.position, _RBTransform.rotation);
        _shakeInstance.StartFadeIn(0);
        _shakeInstance.StartFadeOut(duration);
        AudioCenter.playSound(destroyPlayerSoundId);
        gameObject.SetActive(false);      
    }

    void CreateShield()
    {
        objectPooler.pickFromPool("Shield", transform);
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
           if(!GameState.isShield)
             DestroyPlayer();    
        }
        if (collider.tag == "Platform")
        {
            if (willDie)
            {
               DestroyPlayer();
            } else
            {
                AudioCenter.playSound(jumpSoundId);
                //Correct position if somethink happend 
                _RBLocalPosition.x = collider.transform.localPosition.x;
                _RBLocalPosition.y = collider.transform.localPosition.y + 0.5f;
                _RBLocalRotation = Quaternion.identity;
                GameState.score++;
                gui.UpdateScore();
                onGround = true;
            }
            
        }
        if(collider.tag == "BrokenPlatform")
        {
            willDie = true;
            collider.gameObject.GetComponent<BrokenPlatform>().destroyPlatform();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            //AudioManager.Instance.Play("Coin");
            AudioCenter.playSound(coinSoundId);
            other.GetComponent<Coin>().destroyCoin();
            GameState.countCoins++;
        }
        if (other.tag == "ShieldCoin")
        {
            AudioCenter.playSound(shieldUpSoundId);
            other.GetComponent<ShieldCoin>().destroyShieldCoin();
            GameState.isShield = true;
            CreateShield();
        }
    }

    float parabolic(float x)
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
