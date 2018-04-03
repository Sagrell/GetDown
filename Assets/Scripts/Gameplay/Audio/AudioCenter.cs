using UnityEngine;
using System.Collections.Generic;

public class AudioCenter : MonoBehaviour
{
    public static AudioCenter Instance;

#if UNITY_ANDROID && !UNITY_EDITOR
		public AndroidJavaClass unityActivityClass;
		public AndroidJavaObject activityObj;
		private AndroidJavaObject soundObj;
        Dictionary<string, AndroidJavaObject> musicObjects;

        //SOUNDS
		public void PlaySound( string soundName ) {
            int soundId = soundDic[soundName].id;
            float volume = soundDic[soundName].volume* soundDic[soundName].maxVolume;
            soundObj.Call( "PlaySound", new object[] { soundId, volume } );
		}			
		public int LoadSound( string soundName ) {
			return soundObj.Call<int>( "LoadSound", new object[] { "Resources/Sounds/" +  soundName + ".mp3" } );
		}
        public float GetSoundDuration(string soundName)
        {
            int soundId = soundDic[soundName].id;
            return soundObj.Call<float>( "GetDuration", new object[] { soundId } );
        }
		public void UnloadSound(string soundName) {
            int soundId = soundDic[soundName].id;
            soundObj.Call( "UnloadSound", new object[] { soundId } );
		}
        public void UnloadAll() {
            soundObj.Call("unloadAll");
        }
        
        //MUSIC
        public void SeekToMusic(string musicName, int pos ) {
            musicObjects[musicName].Call("SeekTo", new object[] { pos });
        }
        public void StopMusic(string musicName) {

            musicObjects[musicName].Call("Stop");
        }
        public void SetVolumeMusic(string musicName, float volume ) {
            musicObjects[musicName].Call("SetVolume", new object[] { volume });
        }
        public void SetVolumeAllMusic(float volume) {
            foreach (var musicObj in musicObjects.Values)
            {
                float newVolume = musicObj.Call<float>("GetVolume") * volume;
                musicObj.Call("SetVolume", new object[] { newVolume });
            }
        }
        public float GetDurationMusic(string musicName) {
            return (float)musicObjects[musicName].Call<int>("GetDuration")/1000f;
        }
        public void PlayMusic( string musicName, float fadeDuration ) {
            musicObjects[musicName].Call("stop");
            musicObjects[musicName].Call("prepare");
            musicObjects[musicName].Call("Play", new object[] { fadeDuration });
        }
        public void ResumeMusic( string musicName, float fadeDuration ) {
            musicObjects[musicName].Call("Play", new object[] { fadeDuration });
        }
        public void PauseMusic(string musicName, float fadeDuration) {
            musicObjects[musicName].Call("Pause", new object[] { fadeDuration });
        }

        
#else

    public void PlaySound(string soundName)
    {
        Sound sound = soundDic[soundName];
       
        sound.source.volume = sound.volume * sound.maxVolume;
        sound.source.loop = false;
        sound.source.Play();
    }

    public int LoadSound(string soundName)
    {
        var soundID = soundName.GetHashCode();
        var audioClip = Resources.Load<AudioClip>("Sounds/" + soundName);
        soundDic[soundName].source.clip = audioClip;
        return soundID;
    }
    public float GetSoundDuration(string soundName)
    {
        return soundDic[soundName].source.clip.length;
    }
    public void UnloadSound(string soundName)
    {
        var audioClip = soundDic[soundName].clip;
        Resources.UnloadAsset(audioClip);
        soundDic.Remove(soundName);
    }

    //MUSIC
    public void LoadMusic(string musicName)
    {
        Music music = musicDic[musicName];
        musicSources[musicName].clip = Resources.Load<AudioClip>("Music/" + musicName);
        musicSources[musicName].volume = music.volume* music.maxVolume;
        musicSources[musicName].loop = music.isLooping;
        
    }
    public void SetVolumeAllMusic(float volume)
    {
        foreach (var source in musicSources.Values)
        {
            source.volume = volume*0.2f;
        }
    }

    public void StopMusic(string musicName)
    {
        musicSources[musicName].Stop();
    }
    public float GetDurationMusic(string musicName)
    {
        return musicSources[musicName].clip.length;
    }
    public void PlayMusic(string musicName, float fadeDuration)
    {
        musicSources[musicName].Play();
    }
    public void ResumeMusic(string musicName, float fadeDuration)
    {
        musicSources[musicName].Play();
    }
    public void PauseMusic(string musicName, float fadeDuration)
    {
        musicSources[musicName].Pause();
    }
    public void ResetMusic(string musicName)
    {
        
    }
    
#endif
    public void SetVolumeToSounds(float newVolume)
    {
        foreach (var sound in soundDic.Values)
        {
            sound.volume = newVolume* sound.maxVolume;
        }
    }

    //Sounds
    [Header("Sounds:")]
    public Sound[] sounds;

    //Music
    [Space]
    [Header("Music:")]
    public Music[] musics;

    Dictionary<string, Sound> soundDic = new Dictionary<string, Sound>();
    Dictionary<string, Music> musicDic = new Dictionary<string, Music>();

    Dictionary<string, AudioSource> musicSources;

    UserData data;
    private void Awake()
    {
        
        if(Instance == null)
        {
            data = DataManager.Instance.GetUserData();
            Instance = this;
#if UNITY_ANDROID && !UNITY_EDITOR
			        unityActivityClass =  new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
			        activityObj = unityActivityClass.GetStatic<AndroidJavaObject>( "currentActivity" );
			        soundObj = new AndroidJavaObject( "com.sagrell.androidaudiomanager.Sound", 4, activityObj );

                    musicObjects = new Dictionary<string, AndroidJavaObject>();
                    for (int i = 0; i < musics.Length; i++)
                    {
                        Music music = musics[i];
                        music.volume = data.musicVolume;
                        musicDic[music.name] = music;
                        AndroidJavaObject musicObj = new AndroidJavaObject( "com.sagrell.androidaudiomanager.Music", activityObj ); 
                        musicObj.Call("Load", new object[] { music.name + ".mp3", music.maxVolume, music.isLooping });
                        musicObjects[music.name] = musicObj;
                    }
                    for (int i = 0; i < sounds.Length; i++)
                    {
                        Sound sound = sounds[i];
                        sound.volume = data.soundVolume;
                        sound.id = LoadSound(sound.name);
                        soundDic[sound.name] = sound;
                    }
#else
                    musicSources = new Dictionary<string, AudioSource>();
                    for (int i = 0; i < musics.Length; i++)
                    {
                        AudioSource musicSource = gameObject.AddComponent<AudioSource>();
                        Music music = musics[i];
                        music.volume = data.musicVolume;
                        musicDic[music.name] = music;
                        musicSources[music.name] = musicSource;
                        LoadMusic(music.name);
                    }
                    for (int i = 0; i < sounds.Length; i++)
                    {
                        Sound sound = sounds[i];
                        sound.volume = data.soundVolume;
                        sound.source = gameObject.AddComponent<AudioSource>();
                        soundDic[sound.name] = sound;
                        sound.id = LoadSound(sound.name);
                    }

        #endif
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);


    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnDestroy()
    {
        UnloadAll();
        foreach (var musicObj in musicObjects.Values)
        {
            musicObj.Call("release");
        }
        
    }
#endif

}