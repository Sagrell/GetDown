using UnityEngine;
using System.Collections.Generic;

public class AudioCenter : MonoBehaviour
{
    private static AudioCenter instance;
    private AudioSource audioSource;

#if UNITY_ANDROID && !UNITY_EDITOR
		public static AndroidJavaClass unityActivityClass ;
		public static AndroidJavaObject activityObj ;
		private static AndroidJavaObject soundObj ;
		
		public static void PlaySound( int soundId ) {
			soundObj.Call( "playSound", new object[] { soundId } );
		}
		
		public static void PlaySound( int soundId, float volume ) {
			soundObj.Call( "playSound", new object[] { soundId, volume } );
		}
		
		public static void PlaySound( int soundId, float leftVolume, float rightVolume, int priority, int loop, float rate  ) {
			soundObj.Call( "playSound", new object[] { soundId, leftVolume, rightVolume, priority, loop, rate } );
		}
		
		public static int LoadSound( string soundName ) {
			return soundObj.Call<int>( "loadSound", new object[] { "Resources/Sounds/" +  soundName + ".wav" } );
		}
		
		public static void UnloadSound( int soundId ) {
			soundObj.Call( "unloadSound", new object[] { soundId } );
		}

        public static void UnloadAll() {
            soundObj.Call("unloadAll");
        }
#else
    private Dictionary<int, AudioClip> audioDic = new Dictionary<int, AudioClip>();

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

#endif
    public static int jumpSoundId;
    public static int destroyPlayerSoundId;
    public static int coinCollectsoundId;
    public static int shieldUpSoundId;
    public static int shieldHitSoundId;
    public static int diamondSoundId;

    private void Awake()
    {
        instance = this;
        #if !UNITY_ANDROID || UNITY_EDITOR
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.hideFlags = HideFlags.HideInInspector;
        #else
			unityActivityClass =  new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
			activityObj = unityActivityClass.GetStatic<AndroidJavaObject>( "currentActivity" );
			//soundObj = new AndroidJavaObject( "com.catsknead.androidsoundfix.AudioCenter", 1, activityObj, activityObj );
			soundObj = new AndroidJavaObject( "com.sagrell.soundlatencyfix.AudioCenter", 6, activityObj );
        #endif
        jumpSoundId = AudioCenter.LoadSound("Jump");
        destroyPlayerSoundId = AudioCenter.LoadSound("DestroyPlayer");
        coinCollectsoundId = AudioCenter.LoadSound("CoinCollect");
        shieldUpSoundId = AudioCenter.LoadSound("ShieldUp");
        shieldHitSoundId = AudioCenter.LoadSound("ShieldHit");
        diamondSoundId = AudioCenter.LoadSound("DestroyDiamond");
    }
#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnDestroy()
    {
        UnloadAll();
    }
#endif

}