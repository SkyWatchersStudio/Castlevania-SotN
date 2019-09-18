using UnityEngine;
using Cinemachine;

public class Door : MonoBehaviour
{
    public int[] frameNumbers = new int[2];

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
            FramesList.SwitchFrames(frameNumbers, collision.transform);
    }
}
