using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootSteps : MonoBehaviour
{
    [SerializeField] private List<AudioClip> footSteps = new List<AudioClip>();
    [SerializeField] private float minStep = 3f;
    [SerializeField] private float stepCooldown = 10f;

    private int currentStep = 0;
    private AudioSource footStepSource;
    private bool canPlay = true;


    private void Start()
    {
        footStepSource = GetComponent<AudioSource>();

        Player.PlayerMove.AddListener(EmitSounds);
    }

    private void EmitSounds(Player player, Vector3 velocity)
    {
        float speed = velocity.magnitude;
        float currentSpeed = player.GetCurrentSpeed();

        if (speed < minStep || !player.IsGrounded())
        {
            footStepSource.Stop();
            
            return;
        }

        if (canPlay)
        {
            canPlay = false;

            footStepSource.clip = footSteps[currentStep];

            currentStep++;
            if (currentStep == footSteps.Count)
            {
                currentStep = 0;
            }

            footStepSource.Play();

            Invoke(nameof(ResetSound), stepCooldown / currentSpeed);
        }
    }

    private void ResetSound()
    {
        canPlay = true;
    }
}
