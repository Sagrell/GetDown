using UnityEngine;
using System.Collections.Generic;

public class AudioCenter : MonoBehaviour
{
    private static AudioCenter instance;
    private AudioSource audioSource;

#if UNITY_ANDROID && !UNITY_EDITOR
		public static AndroidJavaClass unityActivityClass;
		public static AndroidJavaObject activityObj;
		private static AndroidJavaObject soundObj;
		private static AndroidJavaObject musicObj;
        

        //SOUNDS
		public static void PlaySound( int soundId ) {
			soundObj.Call( "PlaySound", new object[] { soundId } );
		}		
		public static void PlaySound( int soundId, float volume ) {
			soundObj.Call( "PlaySound", new object[] { soundId, volume } );
		}		
		public static void PlaySound( int soundId, float leftVolume, float rightVolume, int priority, int loop, float rate  ) {
			soundObj.Call( "PlaySound", new object[] { soundId, leftVolume, rightVolume, priority, loop, rate } );
		}		
		public static int LoadSound( string soundName ) {
			return soundObj.Call<int>( "LoadSound", new object[] { "Resources/Sounds/" +  soundName + ".mp3" } );
		}
		public static void UnloadSound( int soundId ) {
			soundObj.Call( "UnloadSound", new object[] { soundId } );
		}
        public static void UnloadAll() {
            soundObj.Call("unloadAll");
        }

        //MUSIC
        public static void LoadMusic( string soundName, float volume, bool isLooping ) {
            musicObj.Call("Load", new object[] { soundName + ".mp3", volume, isLooping });
        }
        public static void SeekToMusic( int pos ) {
            musicObj.Call("SeekTo", new object[] { pos });
        }
        public static void StopMusic( string soundName, float volume, bool isLooping ) {
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
    private Dictionary<int, AudioClip> audioDic = new Dictionary<int, AudioClip>();
    private AudioClip music;
    public static void PlaySound(int soundId)
    {
        //AudioCenter.instance.audioSource.clip = AudioCenter.instance.audioDic[soundId];
        AudioCenter.instance.audioSource.PlayOneShot(AudioCenter.instance.audioDic[soundId]);
    }

    public static void PlaySound(int soundId, float volume)
    {
      
        AudioCenter.instance.audioSource.PlayOneShot(AudioCenter.instance.audioDic[soundId], volume);
    }

    public static void PlaySound(int soundId, float leftVolume, float rightVolume, int priority, int loop, float rate)
    {
        //float panRatio = AudioCenter.instance.audioSource.panStereo;
        //rightVolume = Mathf.Clamp(rightVolume, 0, 1);
        //leftVolume = Mathf.Clamp(leftVolume, 0, 1);
        //AudioCenter.instance.audioSource.panStereo = Mathf.Clamp(rightVolume, 0, 1) - Mathf.Clamp(leftVolume, 0, 1);
        float volume = (leftVolume + rightVolume) / 2;
        AudioCenter.instance.audioSource.PlayOneShot(AudioCenter.instance.audioDic[soundId], volume);
        //AudioCenter.instance.audioSource.panStereo = panRatio;
    }

    public static int LoadSound(string soundName)
    {
        var soundID = soundName.GetHashCode();
        var audioClip = Resources.Load<AudioClip>("Sounds/" + soundName);
        AudioCenter.instance.audioDic[soundID] = audioClip;
        return soundID;
    }

    public static void UnloadSound(int soundId)
    {
        var audioClip = AudioCenter.instance.audioDic[soundId];
        Resources.UnloadAsset(audioClip);
        AudioCenter.instance.audioDic.Remove(soundId);
    }

    //MUSIC
    public static void LoadMusic(string soundName, float volume, bool isLooping)
    {
        AudioCenter.instance.music = Resources.Load<AudioClip>("Sounds/" + soundName);
    }
    public static void StopMusic(string soundName, float volume, bool isLooping)
    {
        AudioCenter.instance.audioSource.Stop();
    }
    public static int GetDurationMusic()
    {
        return (int)(AudioCenter.instance.music.length*1000);
    }
    public static void PlayMusic(float fadeDuration)
    {
        AudioCenter.instance.audioSource.PlayOneShot(AudioCenter.instance.music);
    }
    public static void PauseMusic(float fadeDuration)
    {
        AudioCenter.instance.audioSource.Pause();
    }
#endif
    public static int jumpSoundId;
    public static int destroyPlayerSoundId;
    public static int coinCollectsoundId;
    public static int shieldUpSoundId;
    public static int shieldHitSoundId;
    public static int diamondSoundId;
    public static int laserShotSoundId;
    public static int laserStartSoundId;
    private void Awake()
    {
        instance = this;
        #if !UNITY_ANDROID || UNITY_EDITOR
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.hideFlags = HideFlags.HideInInspector;
#else
			unityActivityClass =  new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
			activityObj = unityActivityClass.GetStatic<AndroidJavaObject>( "currentActivity" );
			soundObj = new AndroidJavaObject( "com.sagrell.androidaudiomanager.Sound", 4, activityObj );
            musicObj = new AndroidJavaObject( "com.sagrell.androidaudiomanager.Music", activityObj );
            AudioCenter.LoadMusic("Rhinoceros", 0.5f, false);
#endif
        jumpSoundId = AudioCenter.LoadSound("Jump");
        destroyPlayerSoundId = AudioCenter.LoadSound("DestroyPlayer");
        coinCollectsoundId = AudioCenter.LoadSound("CoinCollect");
        shieldUpSoundId = AudioCenter.LoadSound("ShieldUp");
        shieldHitSoundId = AudioCenter.LoadSound("ShieldHit");
        diamondSoundId = AudioCenter.LoadSound("DestroyDiamond");
        laserShotSoundId = AudioCenter.LoadSound("LaserShot");
        laserStartSoundId = AudioCenter.LoadSound("LaserStart");

    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnDestroy()
    {
        UnloadAll();
        musicObj.Call("release");
    }
#endif

}