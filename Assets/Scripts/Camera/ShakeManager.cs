using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager : MonoBehaviour {

    static CameraShakeInstance _shakePlayerInstance;
    static CameraShakeInstance _shakeShieldInstance;

    CameraShaker CS;

    // Use this for initialization
    void Start () {
        CS = CameraShaker.Instance;
        _shakePlayerInstance = CS.StartShake(5, 10, 0, new Vector3(0.2f,0.4f), new Vector3(0, 0, 0));
        _shakePlayerInstance.StartFadeOut(0);
        _shakePlayerInstance.DeleteOnInactive = false;

        _shakeShieldInstance = CS.StartShake(3, 10, 0, new Vector3(0.4f, 0.2f), new Vector3(0, 0, 0));
        _shakeShieldInstance.StartFadeOut(0);
        _shakeShieldInstance.DeleteOnInactive = false;
    }
	
	public static void ShakeAfterDeath()
    {
        _shakePlayerInstance.StartFadeIn(0);
        _shakePlayerInstance.StartFadeOut(1);
    }
    public static void ShakeAfterShieldHit()
    {
        _shakeShieldInstance.StartFadeIn(0);
        _shakeShieldInstance.StartFadeOut(.5f);
    }

    private void OnDestroy()
    {
        _shakeShieldInstance = null;
        _shakePlayerInstance = null;
        CS = null;
    }
}
