//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;
using UnityEngine.SceneManagement;


public class SoundKit : Singleton<SoundKit>
{
    [SerializeField, Tooltip("Anytime you play a sound and set the scaledVolume it is multiplied by this value")]
    float DefaultSoundVolume = 1f;
    [SerializeField, Tooltip("Volume which will be set after unmuting")]
    float UnmutedSoundVolume = 1f;
    
    [SerializeField, Tooltip("Anytime you play a music and set the scaledVolume it is multiplied by this value")]
    float DefaultMusicVolume = 1f;
    [SerializeField, Tooltip("Volume which will be set after unmuting")]
    float UnmutedMusicVolume = 1f;

    public static readonly string SoundVolumePrefs = "PLAYER_SOUND_VOLUME";
    public static readonly string MusicVolumePrefs = "PLAYER_MUSIC_VOLUME";

    public int InitialCapacity = 10;
    public int MaxCapacity = 15;
    public bool ClearAllAudioClipsOnLevelLoad = true;
    [NonSerialized]
    public SKSound BackgroundSound;
    float BackgroundVolume = 1f; // BG sound volume, independent from overall bg volume.

    [SerializeField]
    AudioClip InitialBackgroundMusic = null;

    SKSound OneShotSound;

    Stack<SKSound> AvailableSounds;
    List<SKSound> PlayingSounds;


    #region MonoBehaviour

    new void Awake()
    {
        base.Awake();

        LoadVolume();

        // Create the _soundList to speed up sound playing in game
        AvailableSounds = new Stack<SKSound>(MaxCapacity);
        PlayingSounds = new List<SKSound>();

        for (int i = 0; i < InitialCapacity; i++)
            AvailableSounds.Push(new SKSound(this));
    }

    void OnEnable()
    {
        if (InitialBackgroundMusic != null)
            PlayBackgroundMusic(InitialBackgroundMusic);

        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (ClearAllAudioClipsOnLevelLoad)
        {
            for (var i = PlayingSounds.Count - 1; i >= 0; i--)
            {
                var s = PlayingSounds[i];
                s.AudioSource.clip = null;

                AvailableSounds.Push(s);
                PlayingSounds.RemoveAt(i);
            }
        }
    }


    void Update()
    {
        for (var i = PlayingSounds.Count - 1; i >= 0; i--)
        {
            var sound = PlayingSounds[i];
            if (sound.PlayingLoopingAudio)
                continue;

            sound.ElapsedTime += Time.deltaTime;
            if (sound.ElapsedTime > sound.AudioSource.clip.length)
                sound.stop();
        }
    }

    #endregion

    #region Play controls
    /// <summary>
    /// fetches the next available sound and adds the sound to the PlayingSounds List
    /// </summary>
    /// <returns>The available sound.</returns>
    SKSound NextAvailableSound()
    {
        SKSound sound = null;

        if (AvailableSounds.Count > 0)
            sound = AvailableSounds.Pop();

        // if we didnt find an available found, bail out
        if (sound == null)
            sound = new SKSound(this);
        PlayingSounds.Add(sound);

        return sound;
    }


    /// <summary>
    /// starts up the background music and optionally loops it. You can access the SKSound via the BackgroundSound field.
    /// </summary>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="volume">Volume scale.</param>
    /// <param name="loop">If set to <c>true</c> loop.</param>
    public void PlayBackgroundMusic(AudioClip audioClip, float volume = 1.0f, bool loop = true)
    {
        if (audioClip == null)
        {
            Debug.LogError("Null sound played");
            return;
        }

        if (BackgroundSound == null)
            BackgroundSound = new SKSound(this);

        BackgroundVolume = volume;
        BackgroundSound.playAudioClip(audioClip, volume * MusicVolume, 1f, 0f);
        BackgroundSound.setLoop(loop);
    }


    /// <summary>
    /// fetches any AudioSource it can find and uses the standard PlayOneShot to play. Use this if you don't require any
    /// extra control over a clip and don't care about when it completes. It avoids the call to StartCoroutine.
    /// nb. pan/pitch are not supported as the chosen AudioSource might be in use with another pan/pitch setting and Unity does not support setting
    /// them natively in PlayOneShot, so updating them here can result in bad audio.
    /// </summary>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="volumeScale">Volume scale.</param>
    public void PlayOneShot(AudioClip audioClip, float volumeScale = 1f)
    {
        if (audioClip == null)
        {
            Debug.LogError("Null sound played");
            return;
        }

        if (OneShotSound == null)
            OneShotSound = new SKSound(this);

        OneShotSound.AudioSource.PlayOneShot(audioClip, volumeScale * SoundVolume);
    }


    /// <summary>
    /// plays the AudioClip with the default volume (SoundVolume)
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    public SKSound PlaySound(AudioClip audioClip)
    {
        return PlaySound(audioClip, 1f);
    }


    /// <summary>
    /// plays the AudioClip with the specified volume
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="volume">Volume.</param>
    public SKSound PlaySound(AudioClip audioClip, float volume)
    {
        return PlaySound(audioClip, volume, 1f, 0f);
    }


    /// <summary>
    /// plays the AudioClip with the specified pitch
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="pitch">Pitch.</param>
    public SKSound PlayPitchedSound(AudioClip audioClip, float pitch)
    {
        return PlaySound(audioClip, 1f, pitch, 0f);
    }

    /// <summary>
    /// plays the AudioClip with the specified pan
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="pan">Pan.</param>
    public SKSound PlayPannedSound(AudioClip audioClip, float pan)
    {
        return PlaySound(audioClip, 1f, 1f, pan);
    }


    /// <summary>
    /// plays the AudioClip with the specified volumeScale, pitch and pan
    /// </summary>
    /// <returns>The sound.</returns>
    /// <param name="audioClip">Audio clip.</param>
    /// <param name="volume">Volume.</param>
    /// <param name="pitch">Pitch.</param>
    /// <param name="pan">Pan.</param>
    public SKSound PlaySound(AudioClip audioClip, float volumeScale, float pitch, float pan)
    {
        if (audioClip == null)
        {
            Debug.LogError("Null sound played");
            return null;
        }

        // Find the first SKSound not being used. if they are all in use, create a new one
        SKSound sound = NextAvailableSound();
        sound.playAudioClip(audioClip, volumeScale * SoundVolume, pitch, pan);

        return sound;
    }


    /// <summary>
    /// loops the AudioClip. Do note that you are responsible for calling either stop or fadeOutAndStop on the SKSound
    /// or it will not be recycled
    /// </summary>
    /// <returns>The sound looped.</returns>
    /// <param name="audioClip">Audio clip.</param>
    public SKSound PlaySoundLooped(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogError("Null sound played");
            return null;
        }

        // find the first SKSound not being used. if they are all in use, create a new one
        SKSound sound = NextAvailableSound();
        sound.playAudioClip(audioClip, SoundVolume, 1f, 0f);
        sound.setLoop(true);

        return sound;
    }


    /// <summary>
    /// used internally to recycle SKSounds and their AudioSources
    /// </summary>
    /// <param name="sound">Sound.</param>
    public void RecycleSound(SKSound sound)
    {
        // we dont recycle the BackgroundSound since it always stays alive
        if (sound == BackgroundSound || sound == OneShotSound)
            return;

        var index = 0;
        while (index < PlayingSounds.Count)
        {
            if (PlayingSounds[index] == sound)
                break;
            index++;
        }
        PlayingSounds.RemoveAt(index);


        // if we are already over capacity dont recycle this sound but destroy it instead
        if (AvailableSounds.Count + PlayingSounds.Count >= MaxCapacity)
            Destroy(sound.AudioSource);
        else
            AvailableSounds.Push(sound);
    }
    #endregion

    #region Volume settings
    void LoadVolume()
    {
        _SoundVolume = PlayerPrefs.GetFloat(SoundVolumePrefs, DefaultSoundVolume);
        _MusicVolume = PlayerPrefs.GetFloat(MusicVolumePrefs, DefaultMusicVolume);
    }

    public bool SoundEnabled
    {
        get { return SoundVolume > 0; }
        set { SoundVolume = value ? UnmutedSoundVolume : 0f; }
    }
    public float SoundVolume
    {
        get { return _SoundVolume; }
        set
        {
            _SoundVolume = value;
            PlayerPrefs.SetFloat(SoundVolumePrefs, _SoundVolume);

            if (PlayingSounds != null)
            {
                for (int i = 0; i < PlayingSounds.Count; i++)
                {
                    // Warning, we dont consider initial volume of the sound here, every sound will play at the same volume
                    // Should probably save default unscaled value in SKSound
                    PlayingSounds[i].AudioSource.volume = _SoundVolume;
                }
            }
            if (OneShotSound != null)
                OneShotSound.AudioSource.volume = _SoundVolume;
        }
    }
    float _SoundVolume = 1f;

    public bool MusicEnabled
    {
        get { return MusicVolume > 0; }
        set { MusicVolume = value ? UnmutedMusicVolume : 0f; }
    }
    public float MusicVolume
    {
        get { return _MusicVolume; }
        set
        {
            _MusicVolume = value;
            PlayerPrefs.SetFloat(MusicVolumePrefs, _MusicVolume);

            if (BackgroundSound != null)
                BackgroundSound.AudioSource.volume = BackgroundVolume * _MusicVolume;
        }
    }
    float _MusicVolume = 1f;

    #endregion

    #region SKSound inner class

    public class SKSound
    {
        SoundKit Manager;

        public AudioSource AudioSource;
        internal Action CompletionHandler;
        internal bool PlayingLoopingAudio;
        internal float ElapsedTime;


        public SKSound(SoundKit manager)
        {
            Manager = manager;
            AudioSource = Manager.gameObject.AddComponent<AudioSource>();
            AudioSource.playOnAwake = false;
        }

        public bool JustStarted()
        {
            return AudioSource.isPlaying && AudioSource.time == 0;
        }

        /// <summary>
        /// fades out the audio over duration. this will short circuit and stop immediately if the elapsedTime exceeds the clip.length
        /// </summary>
        /// <returns>The out.</returns>
        /// <param name="duration">Duration.</param>
        /// <param name="onComplete">On complete.</param>
        IEnumerator fadeOut(float duration, Action onComplete)
        {
            var startingVolume = AudioSource.volume;

            // fade out the volume
            while (AudioSource.volume > 0.0f && ElapsedTime < AudioSource.clip.length)
            {
                AudioSource.volume -= Time.deltaTime * startingVolume / duration;
                yield return null;
            }

            stop();

            // all done fading out
            if (onComplete != null)
                onComplete();
        }


        /// <summary>
        /// sets whether the SKSound should loop. If true, you are responsible for calling stop on the SKSound to recycle it!
        /// </summary>
        /// <returns>The SKSound.</returns>
        /// <param name="shouldLoop">If set to <c>true</c> should loop.</param>
        public SKSound setLoop(bool shouldLoop)
        {
            PlayingLoopingAudio = true;
            AudioSource.loop = shouldLoop;

            return this;
        }


        /// <summary>
        /// sets an Action that will be called when the clip finishes playing
        /// </summary>
        /// <returns>The SKSound.</returns>
        /// <param name="handler">Handler.</param>
        public SKSound setCompletionHandler(Action handler)
        {
            CompletionHandler = handler;

            return this;
        }


        /// <summary>
        /// stops the audio clip, fires the completionHandler if necessary and recycles the SKSound
        /// </summary>
        public void stop()
        {
            AudioSource.Stop();

            if (CompletionHandler != null)
            {
                CompletionHandler();
                CompletionHandler = null;
            }

            Manager.RecycleSound(this);
        }


        /// <summary>
        /// fades out the audio clip over time. Note that if the clip finishes before the fade completes it will short circuit
        /// the fade and stop playing
        /// </summary>
        /// <param name="duration">Duration.</param>
        /// <param name="handler">Handler.</param>
        public void fadeOutAndStop(float duration, Action handler = null)
        {
            Manager.StartCoroutine
            (
                fadeOut(duration, () =>
                {
                    if (handler != null)
                        handler();
                })
            );
        }


        internal void playAudioClip(AudioClip audioClip, float volume, float pitch, float pan)
        {
            PlayingLoopingAudio = false;
            ElapsedTime = 0;

            // setup the GameObject and AudioSource and start playing
            AudioSource.clip = audioClip;
            AudioSource.volume = volume;
            AudioSource.pitch = pitch;
            AudioSource.panStereo = pan;

            // reset some defaults in case the AudioSource was changed
            AudioSource.loop = false;
            AudioSource.mute = false;

            AudioSource.Play();
        }

    }

    #endregion

}