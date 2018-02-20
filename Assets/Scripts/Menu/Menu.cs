using System;
using UnityEngine;

public abstract class Menu<T> : Menu where T : Menu<T> {

    #region Singleton
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = (T)this;
    }
    protected virtual void OnDestroy()
    {
        Instance = null;
    }
    #endregion

    protected static void Open()
    {
        if(Instance != null)
        {
            return;
        }

        MenuManager.Instance.OpenMenu<T>();
    }

    protected static void Close()
    {
        if (Instance == null)
        {
            return;
        }

        MenuManager.Instance.CloseMenu();
    }
    public override void OnBackPressed()
    {
        Close();
    }
}
public abstract class Menu : MonoBehaviour
{
    public abstract void OnBackPressed();
}