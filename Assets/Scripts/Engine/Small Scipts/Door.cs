using UnityEngine;
using Cinemachine;

public class Door : MonoBehaviour
{
    public GameObject[] frames = new GameObject[2];
    [Space(10)]
    public GameObject[] framesPrefab = new GameObject[2];

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i].activeSelf)
                {
                    var framePosition = frames[i].transform.position;
                    Destroy(frames[i]);

                    frames[i] = Instantiate(
                        framesPrefab[i], framePosition, Quaternion.identity) as GameObject;
                    var vCam = frames[i].GetComponentInChildren<CinemachineVirtualCamera>();
                    vCam.Follow = collision.transform;

                    frames[i].SetActive(false);
                    continue;
                }
                frames[i].SetActive(true);
            }
        }
    }
}
