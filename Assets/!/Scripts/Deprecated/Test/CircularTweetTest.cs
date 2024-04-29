using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularTweetTest : MonoBehaviour
{
    [SerializeField] private RedSpotInteractable m_RedSpotInteractable;

    private void Start()
    {
        StartCoroutine(Utils.WaitAndDo(2f, () =>
        {
            m_RedSpotInteractable.Interact();
        }));
    }
}
