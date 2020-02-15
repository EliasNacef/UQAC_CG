using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Classe qui decrit comment se caracterise un son a partir d'un clip
/// </summary>
[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false;

	[HideInInspector]
	public AudioSource source;

}
