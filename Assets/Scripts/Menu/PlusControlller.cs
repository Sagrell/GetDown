using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusControlller : MonoBehaviour {
    public static PlusControlller Instance;

    private void Awake()
    {
        Instance = this;
    }
    public Button plusButton;
    public float dropSpeed = 10f;
    public Canvas canvas;

    public GameObject signInGoogle;
    public GameObject signOutGoogle;
    public GameObject signInFacebook;
    public GameObject signOutFacebook;
    public GameObject inviteFriendsFacebook;


    bool isDragging;
    bool isOpened;
    RectTransform _transform;
    Vector2 openOffset = new Vector2(0, -640f);
    Vector2 closeOffset = new Vector2(0, 0);
    Vector2 startCursor2D;
    Vector2 startOffset;
    // Use this for initialization
    void Start () {
        _transform = GetComponent<RectTransform>();
        isDragging = false;
        isOpened = false;
        if(!FB.IsLoggedIn)
        {
            SignOutStateFacebook();
        } else
        {
            SignInStateFacebook();
        }
    }
	
    public void SignInStateFacebook()
    {
        signOutFacebook.SetActive(true);
        inviteFriendsFacebook.SetActive(true);
        signInFacebook.SetActive(false);
    }
    public void SignOutStateFacebook()
    {
        signInFacebook.SetActive(true);
        inviteFriendsFacebook.SetActive(false);
        signOutFacebook.SetActive(false);
    }
    public void SignInStateGoogle()
    {
        signInGoogle.SetActive(false);
        signOutGoogle.SetActive(true);
    }
    public void SignOutStateGoogle()
    {
        signInGoogle.SetActive(true);
        signOutGoogle.SetActive(false);
    }
    // Update is called once per frame
    void Update () {
		if(isDragging)
        {
            Vector2 newOffset = ((Vector2)Input.mousePosition - startCursor2D)/ canvas.scaleFactor;
            newOffset.x = 0f;
            newOffset += startOffset;
            if (newOffset.y >= -640f && newOffset.y <= 0f)
            {
                _transform.offsetMin = newOffset;
                _transform.offsetMax = newOffset;
            }
            
        }
        if (_transform.offsetMin.y < -320f)
        {
            isOpened = true;
        }
        else
        {
            isOpened = false;
        }
    }
    public void OnPlusClick()
    {
        if(isOpened)
        {
            DropUpContent();
        } else
        {
            DropDownContent();
        }
    }
    public void DropDownContent()
    {
        if (!FB.IsLoggedIn)
        {
            SignOutStateFacebook();
        }
        else
        {
            SignInStateFacebook();
        }
        StopAllCoroutines();
        StartCoroutine(MoveFromTo(_transform.offsetMin, openOffset));
    }
    public void DropUpContent()
    {
        StopAllCoroutines();
        StartCoroutine(MoveFromTo(_transform.offsetMin, closeOffset));
    }
    public void BeginDrag()
    {
        StopAllCoroutines();
        isDragging = true;
        startCursor2D = Input.mousePosition;
        startOffset = _transform.offsetMin;
    }
    public void EndDrag()
    {
        isDragging = false;
        //if down
        if(_transform.offsetMin.y < -320f)
        {
            DropDownContent();
        } else
        {
            DropUpContent();
        }
    }

    IEnumerator MoveFromTo(Vector2 from, Vector2 to)
    {
        Vector2 newOffset = from;
        while (Mathf.Abs(newOffset.y - to.y)>0.1f)
        {
            
            newOffset.y = Mathf.SmoothStep(newOffset.y, to.y, dropSpeed * Time.deltaTime);
            _transform.offsetMin = newOffset;
            _transform.offsetMax = newOffset;
            yield return null;
        }
        _transform.offsetMin = to;
        _transform.offsetMax = to;
    }
}
