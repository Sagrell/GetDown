using UnityEngine;

public abstract class PowerUp : MonoBehaviour, IPooledObject
{

    public float rotationSpeed = 100f;

    protected Transform _RBTransform;

    [HideInInspector]
    public static Vector3 startPosition;
    [HideInInspector]
    public static Quaternion startRotation;
    private void Awake()
    {
        startPosition = new Vector3(0, 0.6f, 0);
        startRotation = Quaternion.identity;
    }
    private void Start()
    {
        _RBTransform = GetComponent<Rigidbody>().transform;
    }
    public void OnObjectSpawn()
    {
        _RBTransform = GetComponent<Rigidbody>().transform;
    }

    void Update()
    {
        _RBTransform.Rotate(Vector3.up, rotationSpeed * GameManager.deltaTime);
    }

    public abstract void DestroyPowerUp();


}
