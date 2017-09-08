//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public abstract class AnimatorBase : MonoBehaviour
{
    public Action OnAnimationEnd;


    [SerializeField]
    protected SpriteAnimation[] Animations;
    [SerializeField]
    protected SpriteAnimation InitialAnimation;


    public bool Playing { get; private set; }
    public SpriteAnimation CurrentAnimation { get; private set; }
    public int CurrentFrame { get; private set; }
    public bool Loop { get; private set; }


    #region Behaviours
    void OnEnable()
    {
        if (InitialAnimation)
            Play(InitialAnimation);
    }

    void OnDisable()
    {
        Playing = false;
        CurrentAnimation = null;
    }
    #endregion


    protected abstract void SetSprite(Sprite sprite);


    #region Playing
    public void Play(string animationName, bool loop = true, int startFrame = 0, bool warnOnNotFound = true)
    {
        Play(GetAnimation(animationName), loop, startFrame, warnOnNotFound);
    }
    public void Play(int index, bool loop = true, int startFrame = 0, bool warnOnNotFound = true)
    {
        Play(GetAnimation(index), loop, startFrame, warnOnNotFound);
    }
    public void Play(SpriteAnimation animation, bool loop = true, int startFrame = 0, bool warnOnNotFound = true)
    {
        if (animation != null)
        {
            if (animation != CurrentAnimation)
            {
                ForcePlay(animation, loop, startFrame);
            }
        }
        else
        {
            if (warnOnNotFound)
                Debug.LogWarning("Could not find animation");
        }
    }

    public void ForcePlay(string animationName, bool loop = true, int startFrame = 0)
    {
        ForcePlay(GetAnimation(animationName), loop,  startFrame);
    }
    public void ForcePlay(int index, bool loop = true, int startFrame = 0)
    {
        ForcePlay(GetAnimation(index), loop, startFrame);
    }
    public void ForcePlay(SpriteAnimation animation, bool loop = true, int startFrame = 0)
    {
        if (animation != null)
        {
            this.Loop = loop;
            CurrentAnimation = animation;
            Playing = true;
            CurrentFrame = startFrame;
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(CurrentAnimation));
        }
    }

    public void SlipPlay(string animationName, int wantFrame, params string[] otherNames)
    {
        for (int i = 0; i < otherNames.Length; i++)
        {
            if (CurrentAnimation != null && CurrentAnimation.Name == otherNames[i])
            {
                Play(animationName, true, CurrentFrame);
                break;
            }
        }
        Play(animationName, true, wantFrame);
    }
    #endregion


    #region Get parameters
    public bool IsPlaying(string animationName)
    {
        return (CurrentAnimation != null && CurrentAnimation.Name == animationName);
    }

    public bool IsPlaying(SpriteAnimation animation)
    {
        return (CurrentAnimation == animation);
    }


    public SpriteAnimation GetAnimation(string animationName)
    {
        foreach (SpriteAnimation animation in Animations)
        {
            if (animation.Name == animationName)
            {
                return animation;
            }
        }
        return null;
    }

    public SpriteAnimation GetAnimation(int index)
    {
        if (0 <= index && index < Animations.Length)
            return Animations[index];
        return null;
    }
    #endregion


    #region Internal
    IEnumerator PlayAnimation(SpriteAnimation animation)
    {
        if (animation.InitialPause > 0f)
            yield return new WaitForSeconds(animation.InitialPause);

        SetSprite(animation.Frames[CurrentFrame]);

        float timer = 0f;
        float delay = 1f / animation.FPS;
        while (Loop || CurrentFrame < animation.Frames.Count - 1)
        {

            while (timer < delay)
            {
                timer += Time.deltaTime;
                yield return 0f;
            }
            while (timer > delay)
            {
                timer -= delay;
                NextFrame(animation);
            }

            SetSprite(animation.Frames[CurrentFrame]);
        }

        CurrentAnimation = null;

        if (OnAnimationEnd != null)
            OnAnimationEnd();
    }

    void NextFrame(SpriteAnimation animation)
    {
        CurrentFrame++;

        if (CurrentFrame >= animation.Frames.Count)
        {
            if (Loop)
                CurrentFrame = 0;
            else
                CurrentFrame = animation.Frames.Count - 1;
        }

        foreach (var animationTrigger in animation.Triggers)
        {
            if (animationTrigger.Frame == CurrentFrame)
            {
                gameObject.SendMessage(animationTrigger.Tag);
            }
        }
    }
    #endregion


#if UNITY_EDITOR
    [InspectorButton]
    public void UpdateSpriteWithInitial()
    {
        if (Application.isPlaying)
        {
            Debug.LogError("Don't call AnimatorBase.UpdateSpriteWithInitial while playing - this is editor script only");
            return;
        }

        if (InitialAnimation)
        {
            var sprite = InitialAnimation.Frames.First();
            SetSprite(sprite);
            UnityEditor.Undo.RegisterCompleteObjectUndo(gameObject, "Update sprite from animation");
        }
        else
            Debug.LogWarningFormat("Cant update renderer '{0}' with sprite, no initial animation provided", gameObject.name);
    }
#endif
}