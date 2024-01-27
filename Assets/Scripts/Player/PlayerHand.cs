using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [Header("Bobbing")]
    [SerializeField] private Transform playerHead;
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
   
    private float currentTime = 0f;
    private bool canShoot = true;


    private void Start()
    {
        Player.PlayerMove.AddListener(HandBobbing);
    }

    private void HandBobbing(Player player, Vector3 velocity)
    {
        if (velocity.magnitude < minBobbing || !player.IsGrounded())
        {
            currentTime = 0f;
            ResetBobbing();

            return;
        }

        Vector3 wepPos = weaponStaff.localPosition;
        wepPos.y = Mathf.Sin(currentTime) * distanceBobbing;
        wepPos.x = Mathf.Cos(currentTime / 2) * distanceBobbing;

        weaponStaff.localPosition = Vector3.Lerp(weaponStaff.localPosition, wepPos, Time.deltaTime * player.GetCurrentSpeed() * 2f);

        currentTime += Time.deltaTime * bobbingSpeed;
    }

    private void Update()
    {
        Controlls();
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

    private void Shoot()
    {
        if (!canShoot || tagBullets.Count == 0) return;

        GameObject bullet = Instantiate(tagBullets[currentBullet], playerHead.position, playerHead.rotation);

        canShoot = false;
        Invoke(nameof(EnableShooting), shootTime);
    }

    private void EnableShooting()
    {
        canShoot = true;
    }

    private void ResetBobbing()
    {
        Vector3 pos = weaponStaff.localPosition;

        weaponStaff.localPosition = Vector3.Lerp(pos, Vector3.zero, Time.deltaTime * bobbingSpeed);
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
