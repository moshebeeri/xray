using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TGBlinds : MonoBehaviour {
	public string AnimClosed="Closed";
	public string AnimOpened="Opened";
	public string AnimOpen="Open";
	public string AnimClose="Close";
	public AudioClip	AudioOpen;
	public AudioClip	AudioClose;
	public bool 		StartOpened=true;
	[HideInInspector]
	public bool 		Opened;

	Animation anim;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		anim=GetComponent<Animation>();
		audioSource=GetComponent<AudioSource>();
		Opened=StartOpened;
		if (anim && StartOpened) {
			anim.Play(AnimOpened);
		}
	}


	public void Open() {
		if (anim) {
			anim.Play(AnimOpen);
		}
		if (audioSource && AudioOpen) {
			audioSource.PlayOneShot(AudioOpen);
		}
		Opened=true;
	}


	public void Close() {
		if (anim) {
			anim.Play(AnimClose);
		}
		if (audioSource && AudioClose) {
			audioSource.PlayOneShot(AudioClose);
		}
		Opened=false;
	}

	public void Toggle() {
		if (!Opened) {
			Open();	
		}
		else {
			Close();
		}
	}


}

