using UnityEngine.Audio;
using System;
using UnityEngine;

/// <summary>
/// Classe gestionnaire du son
/// </summary>
public class AudioManager : MonoBehaviour
{

	public static AudioManager instance; // Evite que plusieurs Audiomanagers soient charges
	public Sound[] sounds; // Les differents sons disponibles

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
		s.source.Play();
	}

}
