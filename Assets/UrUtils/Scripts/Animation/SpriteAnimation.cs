using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpriteAnimation", menuName = "Animation/Sprite Animation")]
public class SpriteAnimation : ScriptableObject
{
    public string Name;
    public int FPS;
    public float InitialPause = 0.0f;
    public List<Sprite> Frames;

    public List<Trigger> Triggers;


    [Serializable]
    public class Trigger
    {
        public int Frame;
        public string Tag;
    }
}