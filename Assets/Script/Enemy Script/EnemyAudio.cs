using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{


    private AudioSource audioSource;

    [SerializeField]
    private AudioClip scream_Clip, die_Clip;
    
    [SerializeField]
    private AudioClip[] attack_CLips;
   
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Play_ScreamSound()
    {
        audioSource.clip = scream_Clip;
        audioSource.Play();
    }

    public void Play_AttackSound()
    {
        audioSource.clip = attack_CLips[Random.Range(0, attack_CLips.Length)];
        audioSource.Play();
    }
    public void Play_DeadSound()
    {
        audioSource.clip=die_Clip;
        audioSource.Play();
    }


}// class
