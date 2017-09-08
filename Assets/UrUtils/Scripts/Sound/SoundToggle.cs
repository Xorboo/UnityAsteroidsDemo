//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class SoundToggle : MonoBehaviour
{
    [SerializeField]
    Sprite DisabledSprite = null;
    [SerializeField]
    [Tooltip("Sound or Music?")]
    bool IsSound = true;
    [SerializeField]
    bool IsBoth = false;

    Image Image;


    #region Behaviours
    void Awake()
    {
        Image = GetComponent<Image>();
    }

    void OnEnable()
    {
        UpdateImage();
    }
    #endregion


    public void ButtonClicked()
    {
        if (IsSound || IsBoth)
        {
            bool soundEnabled = SoundKit.Instance.SoundEnabled;
            SoundKit.Instance.SoundEnabled = !soundEnabled;
        }
        if (!IsSound || IsBoth)
        {
            bool musicEnabled = SoundKit.Instance.MusicEnabled;
            SoundKit.Instance.MusicEnabled = !musicEnabled;
        }
        UpdateImage();
    }

    void UpdateImage()
    {
        bool soundEnabled = IsSound ? SoundKit.Instance.SoundEnabled : SoundKit.Instance.MusicEnabled;
        Image.overrideSprite = soundEnabled ? null : DisabledSprite;
    }
}
