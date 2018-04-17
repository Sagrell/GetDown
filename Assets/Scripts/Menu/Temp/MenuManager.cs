using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    #region Singleton
    public static MenuManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        MainMenu.Show();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    #endregion
    [Header("Menu prefabs")]
    public MainMenu MainMenuPrefab;
    public ShopMenu ShopMenuPrefab;


    private Stack<Menu> menuStack = new Stack<Menu>();
    public void OpenMenu<T>() where T : Menu
    {
        var prefab = GetPrefab<T>();

        var menuToOpen = Instantiate(prefab, transform);

        if(menuStack.Count > 0)
        {
            menuStack.Peek().gameObject.SetActive(false);
        }

        menuStack.Push(menuToOpen);
    }

    T GetPrefab<T>() where T : Menu
    {
        var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            var prefab = field.GetValue(this) as T;
            if(prefab!=null)
            {
                return prefab;
            }
        }
        throw new MissingReferenceException("Prefab with type "+typeof(T)+" not found");
    }
    public void CloseMenu()
    {
        var menuToClose = menuStack.Pop();
        Destroy(menuToClose);

        if(menuStack.Count > 0 )
        {
            menuStack.Peek().gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 0 )
        {
            menuStack.Peek().OnBackPressed();
        }
    }

}
