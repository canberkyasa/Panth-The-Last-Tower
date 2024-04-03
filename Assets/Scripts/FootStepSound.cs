using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class CharacterFootstep : MonoBehaviour
{
    [Header("Tile Map")]
    [SerializeField] Tilemap tilemap;
    [Header("Audio Clips")]
    [SerializeField] AudioClip[] grassFootstepSounds;
    [SerializeField] AudioClip[] stoneFootstepSounds;
    [SerializeField] AudioClip[] mudFootstepSounds;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Vector3 characterPosition = transform.position;
        Vector3Int cellPosition = tilemap.WorldToCell(characterPosition - new Vector3(0, 0.5f, 0)); // Karakterin altýndaki tile'ý al

        TileBase currentTile = tilemap.GetTile(cellPosition);


        if (currentTile != null && !audioSource.isPlaying)
        {
            PlayFootstepSound(currentTile);
        }
    }

    private void PlayFootstepSound(TileBase currentTile)
    {
        AudioClip footstepSound = null;

        if (currentTile.name == "grass1")
        {
            footstepSound = grassFootstepSounds[Random.Range(0, grassFootstepSounds.Length)];
        }
        else if (currentTile.name == "stone1")
        {
            footstepSound = stoneFootstepSounds[Random.Range(0, stoneFootstepSounds.Length)];
        }
        else if (currentTile.name == "mud1")
        {
            footstepSound = mudFootstepSounds[Random.Range(0, mudFootstepSounds.Length)];
        }

        if (footstepSound != null && isMovement())
        {
            audioSource.clip = footstepSound;
            audioSource.Play();
        }else
        {
            audioSource.Pause();
            audioSource.Stop();
        }
    }

    private Boolean isMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        if (movement.magnitude > 0f)
        {
            return true;
        }
        return false;

    }
}