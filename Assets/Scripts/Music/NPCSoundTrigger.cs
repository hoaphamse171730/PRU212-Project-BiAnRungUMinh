using System.Collections;
using UnityEngine;

public class NPCSoundTrigger : MonoBehaviour
{
    public Transform player;         
    public Transform npc;           
    public AudioSource npcSound;     
    public float triggerDistance = 5f;

    private bool isPlaying = false;
    private bool isFading = false;

    IEnumerator FadeOut()
    {
        isFading = true;
        Debug.Log("Bắt đầu giảm âm thanh");

        while (npcSound.volume > 0)
        {
            npcSound.volume -= Time.deltaTime * 0.5f;
            yield return null;
        }

        npcSound.Stop();
        isPlaying = false;
        isFading = false;
        Debug.Log("Âm thanh đã dừng");
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, npc.position);
        //Debug.Log($"Khoảng cách đến NPC: {distance} | triggerDistance: {triggerDistance}");

        if (distance <= triggerDistance)
        {
            if (!isPlaying)
            {
                npcSound.Play();
                npcSound.volume = 0.3f;
                isPlaying = true;
                isFading = false;
            }
        }
        else
        {
            if (isPlaying && !isFading)
            {
                StartCoroutine(FadeOut());
            }
        }
    }
}
