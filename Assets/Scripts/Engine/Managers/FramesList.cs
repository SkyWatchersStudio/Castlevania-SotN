using UnityEngine;
using Cinemachine;

public class FramesList : MonoBehaviour
{
    public GameObject[] frames;
    public GameObject[] framePrefabs;

    public static FramesList m_Instance;

    private void Start()
    {
        if (m_Instance == null)
            m_Instance = this;
        if (!GameManager.m_Instance.debug)
        {
            //disable all the frames except first one
            for (int i = 1; i < frames.Length; i++)
                frames[i].SetActive(false);
        }
    }

    // with two integer given find the active one and do the job and enable the diactive one
    public static void SwitchFrames(int[] fNum, Transform playerTransform)
    {
        for (int i = 0; i < 2; i++)
        {
            //index of the desire frame
            int listIndex = fNum[i];
            var currentFrame = m_Instance.frames[listIndex];
            bool isActive = currentFrame.activeSelf;

            if (isActive)
            {
                var framePosition = currentFrame.transform.position;
                Destroy(currentFrame.gameObject);

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
