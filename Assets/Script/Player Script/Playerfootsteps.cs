using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Playerfootsteps : MonoBehaviour
{
    private AudioSource footstep_Sound;
    [SerializeField]
    private AudioClip[] footstep_Clip;

    private CharacterController character_Controller;
    [HideInInspector]
    public float volume_Min, volume_Max;
    public float accumalated_Distance;

    [HideInInspector]
    public float step_Distance;

    // Start is called before the first frame update
    void Awake ()
    {
        footstep_Sound = GetComponent<AudioSource>();

        character_Controller = GetComponentInParent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckToPlayFootstepSound();
        
    }

    void CheckToPlayFootstepSound()
    {
        if (!character_Controller.isGrounded)
            return;

        if(character_Controller.velocity.sqrMagnitude > 0)
        {
            accumalated_Distance += Time.deltaTime;

            if (accumalated_Distance > step_Distance)
            {
                footstep_Sound.volume = Random.Range(volume_Min, volume_Max);
                footstep_Sound.clip = footstep_Clip[Random.Range(0, footstep_Clip.Length)];
                footstep_Sound.Play();

                accumalated_Distance = 0f;
            }

        }
        else
        {
            accumalated_Distance = 0f;
        }
    }


}// class






































