//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using System;
using System.Collections;


[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    public Action OnAnimationEnd;

    [Serializable]
    public class AnimationTrigger
    {
        public int frame;
        public string tag;
    }

    [Serializable]
    public class Animation
    {
        public string animationName;
        public int fps;
        public float initialPause = 0.0f;
        public Sprite[] frames;

        public AnimationTrigger[] triggers;
    }
    
    public Animation[] animations;

    public bool playing { get; private set; }
    public Animation currentAnimation { get; private set; }
    public int currentFrame { get; private set; }
    public bool loop { get; private set; }

    public string playAnimationOnStart;

    SpriteRenderer SpriteRenderer
    {
        get
        {
            if (!_SpriteRenderer)
                _SpriteRenderer = GetComponent<SpriteRenderer>();
            return _SpriteRenderer;
        }
    }
    SpriteRenderer _SpriteRenderer = null;


    void OnEnable()
    {
        if (playAnimationOnStart != "")
            Play(playAnimationOnStart);
    }

    void OnDisable()
    {
        playing = false;
        currentAnimation = null;
    }

    public void Play(string animationName, bool loop = true, int startFrame = 0, bool warnOnNotFound = true)
    {
        Play(GetAnimation(animationName), loop, startFrame, warnOnNotFound);
    }
    public void Play(int index, bool loop = true, int startFrame = 0, bool warnOnNotFound = true)
    {
        Play(GetAnimation(index), loop, startFrame, warnOnNotFound);
    }
    public void Play(Animation animation, bool loop = true, int startFrame = 0, bool warnOnNotFound = true)
    {
        if (animation != null)
        {
            if (animation != currentAnimation)
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
    public void ForcePlay(Animation animation, bool loop = true, int startFrame = 0)
    {
        if (animation != null)
        {
            this.loop = loop;
            currentAnimation = animation;
            playing = true;
            currentFrame = startFrame;
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(currentAnimation));
        }
    }

    public void SlipPlay(string animationName, int wantFrame, params string[] otherNames)
    {
        for (int i = 0; i < otherNames.Length; i++)
        {
            if (currentAnimation != null && currentAnimation.animationName == otherNames[i])
            {
                Play(animationName, true, currentFrame);
                break;
            }
        }
        Play(animationName, true, wantFrame);
    }

    public bool IsPlaying(string animationName)
    {
        return (currentAnimation != null && currentAnimation.animationName == animationName);
    }

    public Animation GetAnimation(string animationName)
    {
        foreach (Animation animation in animations)
        {
            if (animation.animationName == animationName)
            {
                return animation;
            }
        }
        return null;
    }

    public Animation GetAnimation(int index)
    {
        if (0 <= index && index < animations.Length)
            return animations[index];
        return null;
    }

    IEnumerator PlayAnimation(Animation animation)
    {
        if (animation.initialPause > 0f)
            yield return new WaitForSeconds(animation.initialPause);

        SpriteRenderer.sprite = animation.frames[currentFrame];

        float timer = 0f;
        float delay = 1f / animation.fps;
        while (loop || currentFrame < animation.frames.Length - 1)
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

            SpriteRenderer.sprite = animation.frames[currentFrame];
        }

        currentAnimation = null;

        if (OnAnimationEnd != null)
            OnAnimationEnd();
    }

    void NextFrame(Animation animation)
    {
        currentFrame++;
        foreach (AnimationTrigger animationTrigger in animation.triggers)
        {
            if (animationTrigger.frame == currentFrame)
            {
                gameObject.SendMessage(animationTrigger.tag);
            }
        }

        if (currentFrame >= animation.frames.Length)
        {
            if (loop)
                currentFrame = 0;
            else
                currentFrame = animation.frames.Length - 1;
        }
    }

    public int GetFacing()
    {
        return (int)Mathf.Sign(SpriteRenderer.transform.localScale.x);
    }

    public void FlipTo(float dir)
    {
        if (dir < 0f)
            SpriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            SpriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void FlipTo(Vector3 position)
    {
        float diff = position.x - transform.position.x;
        if (diff < 0f)
            SpriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            SpriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
