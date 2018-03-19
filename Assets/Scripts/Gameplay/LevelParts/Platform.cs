using UnityEngine;

public class Platform : MonoBehaviour, IPooledObject {


    public void OnObjectSpawn()
    {
        
    }

    public void BlowUp()
    {
        ElementsPool.Instance.PickFromPool("PlatformBlowEffect", transform.parent);
    }
}
