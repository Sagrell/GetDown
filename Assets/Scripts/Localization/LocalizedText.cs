using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

    public string key;

    Text text;
    TextMesh textMesh;
	// Use this for initialization
	void Start () {
        SetText();
    }
	
    public void SetText()
    {
        text = GetComponent<Text>();
        if (text != null)
        {
            text.text = LanguageManager.Instance.GetLocalizedValue(SceneManager.GetActiveScene().name, key);
        }
        else
        {
            textMesh = GetComponent<TextMesh>();
            textMesh.text = LanguageManager.Instance.GetLocalizedValue(SceneManager.GetActiveScene().name, key);
        }
    }
}
