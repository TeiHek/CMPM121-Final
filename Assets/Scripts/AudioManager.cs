using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private List<AudioClip> footstepSounds;
    [SerializeField] private AudioSource gun;
    [SerializeField] private AudioClip gunSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstep()
    {
        footstep.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Count)]);
    }

    public void PlayShot()
    {
        gun.PlayOneShot(gunSound);
    }
}
