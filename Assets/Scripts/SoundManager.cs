using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	public AudioSource audioSource;

	public AudioSource musicSource;

	public AudioClip music;

	public AudioClip sfxPlay;

	public AudioClip sfxGeneric;

	public AudioClip sfxGameOverHiScore;

	public AudioClip sfxGameOver;

	public AudioClip sfxMoveFail;

	public AudioClip sfxMoveSuccess;

	public AudioClip sfxNewBatch;

	public AudioClip sfxNewRecord;

	public AudioClip[] sfxClear1;

	public AudioClip[] sfxClear23;

	public AudioClip[] sfxClear4;

	public AudioClip[] sfxClear5;

	public AudioClip itemSlotAppear;

	public AudioClip itemSlotRandom;

	public AudioClip itemSlotGet;

	public float sfxMoveVolume;

	private int clearID;

	private void Start()
	{
		if (music != null)
		{
			musicSource.clip = music;
			musicSource.loop = true;
		}
	}

	public void SoundOn()
	{
		audioSource.mute = false;
		musicSource.mute = false;
	}

	public void SoundOff()
	{
		audioSource.mute = true;
		musicSource.mute = true;
	}

	public void PlayButton()
	{
		audioSource.PlayOneShot(sfxGeneric);
	}

	public void PlayNewBatch()
	{
		audioSource.PlayOneShot(sfxNewBatch);
	}

	public void PlayMoveSuccess()
	{
		audioSource.PlayOneShot(sfxMoveSuccess, sfxMoveVolume);
	}

	public void PlayMoveFailed()
	{
		audioSource.PlayOneShot(sfxMoveFail, sfxMoveVolume);
	}

	public void PlayExplosion()
	{
	}

	public void PlayItemSlotAppear()
	{
		audioSource.PlayOneShot(itemSlotAppear);
	}

	public void PlayItemSlotRandom()
	{
		audioSource.PlayOneShot(itemSlotRandom);
	}

	public void PlayItemSlotGet()
	{
		audioSource.PlayOneShot(itemSlotGet);
	}

	public void PlayGetDiamond()
	{
	}

	public void PlayOutOfSlot()
	{
	}

	public void PlayStart()
	{
		audioSource.PlayOneShot(sfxPlay);
	}

	public void PlayHighScore()
	{
		audioSource.PlayOneShot(sfxNewRecord);
	}

	public void PlayGameOver(bool isNewRecord)
	{
		if (isNewRecord)
		{
			audioSource.PlayOneShot(sfxGameOverHiScore);
		}
		else
		{
			audioSource.PlayOneShot(sfxGameOver);
		}
	}

	public void PlayClear(int countClear)
	{
		AudioClip[] array;
		switch (countClear)
		{
		case 1:
			array = sfxClear1;
			break;
		case 2:
			array = sfxClear23;
			break;
		case 3:
			array = sfxClear23;
			break;
		case 4:
			array = sfxClear4;
			break;
		case 5:
			array = sfxClear5;
			break;
		case 6:
			array = sfxClear5;
			break;
		default:
			array = sfxClear5;
			break;
		}
		int num = Random.Range(0, array.Length);
		audioSource.PlayOneShot(array[num]);
	}
}
