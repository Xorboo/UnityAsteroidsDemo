using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class MenuManager : Singleton<MenuManager>
{
    BaseMenu CurrentMenu;
    BasePopup CurrentPopup;


    #region Behaviours
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentPopup)
                CurrentPopup.EscapePressed();
            else if (CurrentMenu)
                CurrentMenu.EscapePressed();
        }
    }
    #endregion


    #region Menu
    public void ShowMenu(BaseMenu menu)
    {
        if (CurrentMenu != null)
            CurrentMenu.Hide();

        CurrentMenu = menu;
        Debug.LogFormat("Showing menu '{0}'", CurrentMenu.gameObject.name);
        ////
        //if (CurrentMenu.gameObject.name == "Settings Menu")
        //{
        //    PlayerPrefs.DeleteAll();
        //    Debug.Log("PlayerPrefs cleared. Now reboot.");
        //}
        ////
        CurrentMenu.Show();
    }
    #endregion


    #region Popup
    public void ShowPopup(BasePopup popup, object data = null)
    {
        HidePopup();

        CurrentPopup = popup;
        Debug.LogFormat("Showing popup '{0}'", CurrentPopup.gameObject.name);
        CurrentPopup.Show(data);
    }

    public void HidePopup()
    {
        if (CurrentPopup)
        {
            Debug.LogFormat("Closing popup '{0}'", CurrentPopup.gameObject.name);
            CurrentPopup.Hide();
            CurrentPopup = null;
        }
    }
    #endregion


    [InspectorButton]
    public void HideAllMenus()
    {
        var menus = GameObject.FindObjectsOfType<BaseMenu>();
        foreach (var menu in menus)
            menu.gameObject.SetActive(false);

        var popups = GameObject.FindObjectsOfType<BasePopup>();
        foreach (var popup in popups)
            popup.gameObject.SetActive(false);

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var objects = new List<UnityEngine.Object>(menus.Length + popups.Length);
            objects.AddRange(menus);
            objects.AddRange(popups);
            UnityEditor.Undo.RecordObjects(objects.ToArray(), "Disabling Menus");
        }
#endif
    }
}
