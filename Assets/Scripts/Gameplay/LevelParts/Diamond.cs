using UnityEngine;

public class Diamond : Enemy, IPooledObject
{

    public float speed = 5f;
    public float rotationSpeed = 10f;

    float startSpeed;
    Transform _RBTransform;
    Vector3 currPosition;
    Vector3 rightPosition;
    Vector3 leftPosition;
    Vector3 nextPosition;

    // Use this for initialization
    void Start () {
        _RBTransform = GetComponent<Rigidbody>().transform;
        currPosition = _RBTransform.localPosition;
        leftPosition = new Vector3(-4f, currPosition.y);
        rightPosition = new Vector3(4f, currPosition.y);
        nextPosition = rightPosition;
        startSpeed = speed;
    }

    public void OnObjectSpawn()
    {
        _RBTransform = GetComponent<Rigidbody>().transform;
        currPosition = _RBTransform.localPosition;
        leftPosition = new Vector3(-4f, currPosition.y);
        rightPosition = new Vector3(4f, currPosition.y);
        nextPosition = rightPosition;
    }

    // Update is called once per frame
    void Update () {
        speed = startSpeed * GameState.currentSpeedFactor;
        Move();
        _RBTransform.localPosition = currPosition;
    }

    void Move()
    {
        if(Vector3.Distance(currPosition, nextPosition) < 0.1f)
        {
            SwitchNextPosition();
        }
        currPosition = Vector3.MoveTowards(currPosition, nextPosition, speed * GameManager.deltaTime);
        _RBTransform.Rotate(Vector3.up, rotationSpeed* GameManager.deltaTime);
    }
    void SwitchNextPosition()
    {
        nextPosition = nextPosition != leftPosition ? leftPosition : rightPosition;
    }

    public override void Destroy()
    {
        AudioCenter.Instance.PlaySound("DiamondDestroy");
        ElementsPool.Instance.PickFromPool("DestroyedDiamond", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }


}
