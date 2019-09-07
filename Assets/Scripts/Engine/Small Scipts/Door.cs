using UnityEngine;
using Cinemachine;

public class Door : MonoBehaviour
{
    public int[] frameNumbers = new int[2];
    private static FramesList m_List;

    private void Awake()
    {
        if (m_List == null)
            m_List = GetComponentInParent<FramesList>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < 2; i++)
            {
                var frame = m_List.frames[frameNumbers[i]];
                bool isActive = frame.activeSelf;

                if (isActive)
                {
                    var framePosition = frame.transform.position;
                    Destroy(frame);

                    m_List.frames[frameNumbers[i]] = Instantiate(
                        m_List.framePrefabs[frameNumbers[i]],
                        framePosition, Quaternion.identity) as GameObject;
                    frame = m_List.frames[frameNumbers[i]];

                    var vCam = frame.GetComponentInChildren<CinemachineVirtualCamera>();
                    vCam.Follow = collision.transform;
                }

                frame.SetActive(!isActive);
            }
        }
    }
}
