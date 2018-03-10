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
        startPosition = transform.position;
        startRotation = transform.rotation;
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
