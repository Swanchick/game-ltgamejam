using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [Header("Bobbing")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform weaponStaff;
    [SerializeField] private Player player;
    [SerializeField] private float minBobbing = 0f;
    [SerializeField] private float distanceBobbing = 0.5f;
    [SerializeField] private float bobbingSpeed = 5f;

    [Header("Staff")]
    [SerializeField] private float shootTime = 0.2f;
    [SerializeField] private List<GameObject> tagBullets = new();
    [SerializeField] private int currentBullet = 0;

    [Header("UI")]
    [SerializeField] private RectTransform tagPanel;

    [Header("Sound")]
    private float soundCooldown = 0.5f;

    private float currentTime = 0f;
    private bool canShoot = true;

    private AudioSource audioSource;
    private bool canPlay = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Player.PlayerMove.AddListener(HeadBobbing);
    }

    private void HeadBobbing(Player player, Vector3 velocity)
    {
        if (velocity.magnitude < minBobbing || !player.IsGrounded())
        {
            currentTime = 0f;
            ResetBobbing();

            return;
        }

        Vector3 camPos = playerCamera.localPosition;
        camPos.y = Mathf.Sin(currentTime) * distanceBobbing;

        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, camPos, Time.deltaTime * player.GetCurrentSpeed() * 2f);

        currentTime += Time.deltaTime * bobbingSpeed;
    }

    private void ResetBobbing()
    {
        Vector3 pos = playerCamera.localPosition;

        playerCamera.localPosition = Vector3.Lerp(pos, Vector3.zero, Time.deltaTime * bobbingSpeed);
    }

    private void Update()
    {
        Controlls();
        EmitSound();
    }

    private void Controlls()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        currentBullet = GetInventory();
    }

    private int GetInventory()
    {
        string inputString = Input.inputString;
        if (inputString.Length == 0) return currentBullet;

        char currentChar = inputString[0];
        if (!char.IsDigit(currentChar)) return currentBullet;

        int inv = Mathf.Clamp(Convert.ToInt16(currentChar) - 48, 1, tagBullets.Count) - 1;

        Debug.Log(inv);

        return inv;
    }

    private void EmitSound()
    {
        if (tagBullets.Count == 0) return;

        if (Input.GetMouseButtonDown(0))
        {
            audioSource.Play();
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            audioSource.Stop();
        }
    }

    private void Shoot()
    {
        if (!canShoot || tagBullets.Count == 0) return;

        GameObject bullet = Instantiate(tagBullets[currentBullet], playerCamera.position, playerCamera.rotation);

        canShoot = false;
        Invoke(nameof(EnableShooting), shootTime);
    }

    private void EnableShooting()
    {
        canShoot = true;
    }

    public bool TagExist(string tagName)
    {
        foreach (GameObject tagObject in tagBullets)
        {
            if (tagObject.name == tagName)
                return true;
        }

        return false;
    }

    public void AddTag(GameObject tagObject, GameObject tagImage)
    {
        if (TagExist(tagObject.name)) return;

        tagBullets.Add(tagObject);
        Instantiate(tagImage, tagPanel);

        currentBullet = tagBullets.Count - 1;
    }
}
