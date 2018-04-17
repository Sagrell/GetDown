using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : Menu<ShopMenu> {

    public static void Show()
    {
        Open();

    }
    public static void Hide()
    {
        Close();
    }
    public override void OnBackPressed()
    {
        Hide();
    }
}
