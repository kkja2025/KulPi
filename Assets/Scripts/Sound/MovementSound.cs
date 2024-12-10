using UnityEngine;
using UnityEngine.EventSystems;

public class MovementSound : OnScreenButtonWithSound, IPointerUpHandler
{
    private PlayerMovement playerMovement;
    private bool isButtonHeld = false;

    protected override void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        soundEffectsSource = AudioManager.Singleton.GetSoundEffectsSource();
        soundEffectsSource.clip = clickSound;
        soundEffectsSource.loop = true;
    }
    
    private void Update()
    {
        if (playerMovement == null)
        {
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
        if (soundEffectsSource == null)
        {
            soundEffectsSource = AudioManager.Singleton.GetSoundEffectsSource();
            soundEffectsSource.clip = clickSound;
            soundEffectsSource.loop = true;
        }

        if (isButtonHeld)
        {
            if (playerMovement.isGrounded && !soundEffectsSource.isPlaying)
            {
                soundEffectsSource.Play();
            }
            else if (!playerMovement.isGrounded)
            {
                soundEffectsSource.Stop();
            }
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isButtonHeld = true;
        if (clickSound.name == "Jump")
        {
            if (playerMovement.isGrounded)
            {
                soundEffectsSource.PlayOneShot(clickSound);
            }
        }    
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonHeld = false;
        soundEffectsSource.Stop(); 
    }
}
