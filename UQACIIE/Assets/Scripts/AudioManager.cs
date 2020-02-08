using UnityEngine.Audio;
using System;
using UnityEngine;

/// <summary>
/// Classe gestionnaire du son
/// </summary>
public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	//public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

			//s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

    /// <summary>
    /// Jouer un son
    /// </summary>
    /// <param name="sound"> Le son a jouer </param>
    public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Le son " + name + " n'a pas été trouvé..");
			return;
		}

		//s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		//s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

}
