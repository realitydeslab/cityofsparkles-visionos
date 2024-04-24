using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialState
{
    ReachingFirstSpot = 0,
    ReachingSecondSpot = 1,
    Completed = 2
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private RedSpotController m_FirstSpot;

    private TutorialState m_CurrentState = TutorialState.ReachingFirstSpot;
}
