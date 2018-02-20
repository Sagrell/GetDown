using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
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
        Application.Quit();
    }
}
