using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTag : MonoBehaviour
{
    [SerializeField] private GameObject bulletTag;
    [SerializeField] private GameObject imageTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        Player player = other.GetComponent<Player>();
        if (!player) return;

        player.AddTag(bulletTag, imageTag);

        Debug.Log("Hello World");

        Destroy(gameObject);
    }
}
