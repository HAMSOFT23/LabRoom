using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;

    [Header("Settings")]
    [SerializeField] private float footstepRate = 0.5f;
    [SerializeField] private float minMovementSpeed = 0.1f;
    [SerializeField] private float footstepVolumeModifier = 1f;

    // Referencias
    private CharacterController charController;
    private AudioSource audioSource;
    
    // Estado
    private bool wasGrounded;
    private Vector3 prevPosition;
    private float footstepTimer;

    private void Awake()
    {
        // Obtener componentes
        charController = GetComponent<CharacterController>();
        
        // Buscar el AudioSource único existente
        audioSource = GetComponentInChildren<AudioSource>();
        
        if (audioSource == null)
        {
            Debug.LogWarning("No se encontró un AudioSource en los hijos del jugador. Creando uno nuevo.");
            GameObject audioObj = new GameObject("PlayerAudioSource");
            audioObj.transform.parent = transform;
            audioObj.transform.localPosition = Vector3.zero;
            audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1.0f; // Sonido 3D
        }

        // Inicializar estado
        wasGrounded = charController.isGrounded;
        prevPosition = transform.position;
        footstepTimer = 0f;
    }

    private void Update()
    {
        DetectJumpAndLand();
        DetectFootsteps();
    }

    private void DetectJumpAndLand()
    {
        // Detectar salto (indirectamente al ver cuando el jugador deja el suelo)
        if (wasGrounded && !charController.isGrounded)
        {
            PlayJumpSound();
        }

        // Detectar aterrizaje
        if (!wasGrounded && charController.isGrounded)
        {
            PlayLandSound();
        }

        // Actualizar estado
        wasGrounded = charController.isGrounded;
    }

    private void DetectFootsteps()
    {
        if (!charController.isGrounded) return;

        // Calcular movimiento horizontal desde el último frame
        Vector3 horizontalMove = new Vector3(
            transform.position.x - prevPosition.x,
            0f,
            transform.position.z - prevPosition.z
        );
        
        float movementSpeed = horizontalMove.magnitude / Time.deltaTime;

        // Reproducir pasos si está moviéndose y en el suelo
        if (movementSpeed > minMovementSpeed)
        {
            footstepTimer -= Time.deltaTime;
            
            // Calcular frecuencia de pasos basada en velocidad
            float stepFrequency = Mathf.Lerp(footstepRate * 1.5f, footstepRate * 0.5f, Mathf.Clamp01(movementSpeed / 10f));
            
            if (footstepTimer <= 0f)
            {
                PlayFootstepSound(Mathf.Clamp01(movementSpeed / 10f));
                footstepTimer = stepFrequency;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        // Actualizar posición
        prevPosition = transform.position;
    }

    private void PlayFootstepSound(float volumeMultiplier = 1f)
    {
        if (footstepSounds == null || footstepSounds.Length == 0 || audioSource == null) return;

        // Seleccionar sonido de paso aleatorio
        int index = Random.Range(0, footstepSounds.Length);
        if (footstepSounds[index] == null) return;

        // Reproducir el sonido
        audioSource.volume = Mathf.Clamp01(volumeMultiplier * footstepVolumeModifier);
        audioSource.PlayOneShot(footstepSounds[index]);
    }

    private void PlayJumpSound()
    {
        if (jumpSound == null || audioSource == null) return;
        audioSource.PlayOneShot(jumpSound);
    }

    private void PlayLandSound()
    {
        if (landSound == null || audioSource == null) return;
        audioSource.PlayOneShot(landSound);
    }
}