using UnityEngine;
using Cinemachine;

public class FramesList : MonoBehaviour
{
    public GameObject[] frames;
    public GameObject[] framePrefabs;

    public static FramesList m_Instance;

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
    }

    // with two integer given find the active one and do the job and enable the diactive one
    public static void SwitchFrames(int[] fNum, Transform playerTransform)
    {
        for (int i = 0; i < 2; i++)
        {
            //as we work with natuaral numbers in the inspecter we calculate the number
            //we want to work with arrays
            int listIndex = fNum[i] - 1;
            var currentFrame = m_Instance.frames[listIndex];
            bool isActive = currentFrame.activeSelf;

            if (isActive)
            {
                var framePosition = currentFrame.transform.position;
                Destroy(currentFrame);

                //assign reference to the new game object instance
                m_Instance.frames[listIndex] = (GameObject)Instantiate(
                    m_Instance.framePrefabs[listIndex],
                    framePosition, Quaternion.identity);

                currentFrame = m_Instance.frames[listIndex];
                var vCam = currentFrame.GetComponentInChildren<CinemachineVirtualCamera>();
                vCam.Follow = playerTransform; //give the new instance vCam the player target
            }

            //if enabled, disable it and so on
            currentFrame.SetActive(!isActive);
        }
    }
}
