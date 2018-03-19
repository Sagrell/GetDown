using UnityEngine;
using System.Collections.Generic;

public class AudioCenter : MonoBehaviour
{
    private static AudioCenter instance;

#if UNITY_ANDROID && !UNITY_EDITOR
		public static AndroidJavaClass unityActivityClass;
		public static AndroidJavaObject activityObj;
		private static AndroidJavaObject soundObj;
		private static AndroidJavaObject musicObj;

        //SOUNDS
		public static void PlaySound( string soundName ) {
            int soundId = AudioCenter.instance.soundDic[soundName].id;
            float volume = AudioCenter.instance.soundDic[soundName].volume;
            soundObj.Call( "PlaySound", new object[] { soundId, volume } );
		}			
		public static int LoadSound( string soundName ) {
			return soundObj.Call<int>( "LoadSound", new object[] { "Resources/Sounds/" +  soundName + ".mp3" } );
		}
		public static void UnloadSound(string soundName) {
            int soundId = AudioCenter.instance.soundDic[soundName].id;
            soundObj.Call( "UnloadSound", new object[] { soundId } );
		}
        public static void UnloadAll() {
            soundObj.Call("unloadAll");
        }

        //MUSIC
        public static void LoadMusic(string musicName) {
            float volume = AudioCenter.instance.musicDic[musicName].volume;
            bool isLooping = AudioCenter.instance.musicDic[musicName].isLooping;
            musicObj.Call("Load", new object[] { musicName + ".mp3", volume, isLooping });
        }
        public static void SeekToMusic( int pos ) {
            musicObj.Call("SeekTo", new object[] { pos });
        }
        public static void StopMusic() {
            musicObj.Call("Stop");
        }
        public static void SetVolumeMusic( float volume ) {
            musicObj.Call("SetVolume", new object[] { volume });
        }
        public static int GetDurationMusic() {
            return musicObj.Call<int>("GetDuration");
        }
        public static void PlayMusic( float fadeDuration ) {
            musicObj.Call("Play", new object[] { fadeDuration });
        }
        public static void PauseMusic( float fadeDuration) {
            musicObj.Call("Pause", new object[] { fadeDuration });
        }
#else

    public static void PlaySound(string soundName)
    {
        Sound sound = AudioCenter.instance.soundDic[soundName];
       
        sound.source.volume = sound.volume;
        sound.source.loop = false;
        sound.source.Play();
    }

    public static int LoadSound(string soundName)
    {
        var soundID = soundName.GetHashCode();
        var audioClip = Resources.Load<AudioClip>("Sounds/" + soundName);
        AudioCenter.instance.soundDic[soundName].source.clip = audioClip;
        return soundID;
    }

    public static void UnloadSound(string soundName)
    {
        var audioClip = AudioCenter.instance.soundDic[soundName].clip;
        Resources.UnloadAsset(audioClip);
        AudioCenter.instance.soundDic.Remove(soundName);
    }

    //MUSIC
    public static void LoadMusic(string musicName)
    {
        Music music = AudioCenter.instance.musicDic[musicName];
        AudioCenter.instance.musicSource.clip = Resources.Load<AudioClip>("Music/" + musicName);
        AudioCenter.instance.musicSource.volume = music.volume;
        AudioCenter.instance.musicSource.loop = music.isLooping;
        
    }
    public static void StopMusic()
    {
        AudioCenter.instance.musicSource.Stop();
    }
    public static int GetDurationMusic()
    {
        return (int)(AudioCenter.instance.musicSource.clip.length*1000);
    }
    public static void PlayMusic(float fadeDuration)
    {
        AudioCenter.instance.musicSource.Play();
    }
    public static void PauseMusic(float fadeDuration)
    {
        AudioCenter.instance.musicSource.Pause();
    }
#endif

    //Sounds
    [Header("Sounds:")]
    public Sound[] sounds;

    //Music
    [Space]
    [Header("Music:")]
    public Music[] musics;

    private Dictionary<string, Sound> soundDic = new Dictionary<string, Sound>();
    private Dictionary<string, Music> musicDic = new Dictionary<string, Music>();

    private AudioSource musicSource;
    private void Awake()
    {
        instance = this;
#if UNITY_ANDROID && !UNITY_EDITOR
			unityActivityClass =  new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
			activityObj = unityActivityClass.GetStatic<AndroidJavaObject>( "currentActivity" );
			soundObj = new AndroidJavaObject( "com.sagrell.androidaudiomanager.Sound", 4, activityObj );
            musicObj = new AndroidJavaObject( "com.sagrell.androidaudiomanager.Music", activityObj ); 

            for (int i = 0; i < musics.Length; i++)
            {
                Music music = musics[i];
                musicDic[music.name] = music;
            }
            for (int i = 0; i < sounds.Length; i++)
            {
                Sound sound = sounds[i];
                sound.id = AudioCenter.LoadSound(sound.name);
                soundDic[sound.name] = sound;
            }
#else

        musicSource = gameObject.AddComponent<AudioSource>();
        for (int i = 0; i < musics.Length; i++)
        {
            Music music = musics[i];
            musicDic[music.name] = music;
        }
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            sound.source = gameObject.AddComponent<AudioSource>();
            soundDic[sound.name] = sound;
            sound.id = AudioCenter.LoadSound(sound.name);
        }

#endif

    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnDestroy()
    {
        UnloadAll();
        musicObj.Call("release");
    }
#endif

}