using UnityEngine;
using System.Collections;

public class WaterSoundTrigger : MonoBehaviour
{
    public AudioSource waterSound;  // Kéo AudioSource của nước vào đây
    public Transform player;  // Kéo GameObject Player vào đây
    public float triggerDistance = 5f;  // Khoảng cách để phát âm thanh
    private bool isPlaying = false; // Kiểm tra trạng thái âm thanh
    private bool isFading = false;  // Kiểm tra có đang giảm âm hay không

    IEnumerator FadeOut()
    {
        isFading = true;
        Debug.Log("Bắt đầu giảm âm");

        while (waterSound.volume > 0)
        {
            waterSound.volume -= Time.deltaTime * 0.5f;
            yield return null;
        }

        waterSound.Stop();
        isPlaying = false;
        isFading = false;
        Debug.Log("Âm thanh đã dừng");
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Khoảng cách đến nước: {distance} | triggerDistance: {triggerDistance}");

        if (distance <= triggerDistance)
        {
            if (!isPlaying)
            {
                waterSound.Play();
                waterSound.volume = 0.3f;
                isPlaying = true;
                isFading = false; // Reset trạng thái giảm âm
            }
        }
        else
        {
            if (isPlaying && !isFading) // Chỉ gọi FadeOut() khi cần
            {
                StartCoroutine(FadeOut());
            }
        }
    }
}
