using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Clips")]
    public AudioClip itemAccept;
    public AudioClip itemReject;
    public AudioClip itemCollect;
    public AudioClip pickaxeUpgrade;
    public AudioClip bagUpgrade;
    public AudioClip buttonClick;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip wallJump;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // --- Public Methods ---
    public void PlayItemAccept() => PlaySound(itemAccept);
    public void PlayItemReject() => PlaySound(itemReject);
    public void PlayItemCollect() => PlaySound(itemCollect);
    public void PlayPickaxeUpgrade() => PlaySound(pickaxeUpgrade);
    public void PlayBagUpgrade() => PlaySound(bagUpgrade);
    public void PlayButtonClick() => PlaySound(buttonClick);
    public void PlayJump() => PlaySound(jump);
    public void PlayDash() => PlaySound(dash);
    public void PlayWallJump() => PlaySound(wallJump);

    public void StartPickaxeUpgradeLoop()
    {
        if (pickaxeUpgrade != null)
        {
            audioSource.clip = pickaxeUpgrade;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopPickaxeUpgradeLoop()
    {
        if (audioSource.clip == pickaxeUpgrade && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }
}
