using UnityEngine;
using Cinemachine;

public class Door : MonoBehaviour
{
    public int[] frameNumbers = new int[2];

    private static GameObject[] m_FramesList, m_PrefabsList;

    private void Awake()
    {
        if (m_FramesList != null)
            return;

        var list = GetComponentInParent<FramesList>();
        m_FramesList = list.frames;
        m_PrefabsList = list.framePrefabs;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < 2; i++)
            {
                int listIndex = frameNumbers[i] - 1;
                var currentFrame = m_FramesList[listIndex];
                bool isActive = currentFrame.activeSelf;

                if (isActive)
                {
                    var framePosition = currentFrame.transform.position;
                    Destroy(currentFrame);

                    m_FramesList[listIndex] = Instantiate(
                        m_PrefabsList[listIndex],
                        framePosition, Quaternion.identity) as GameObject;

                    currentFrame = m_FramesList[listIndex];

                    var vCam = currentFrame.GetComponentInChildren<CinemachineVirtualCamera>();
                    vCam.Follow = collision.transform;
                }

                currentFrame.SetActive(!isActive);
            }
        }
    }
}
