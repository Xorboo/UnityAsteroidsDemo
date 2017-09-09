using UnityEngine;


public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip ButtonClick = null;


    #region Behaviours
    void OnEnable()
    {
        SoundButton.OnButtonClicked += ButtonClicked;
    }

    void OnDisable()
    {
        SoundButton.OnButtonClicked -= ButtonClicked;
    }
    #endregion


    void ButtonClicked(SoundButton button)
    {
        SoundKit.Instance.PlayOneShot(ButtonClick);
    }
}
