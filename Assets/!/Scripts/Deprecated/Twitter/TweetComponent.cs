using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TweetType
{
    Circular = 0,
    Rising = 1
}

public class TweetComponent : MonoBehaviour
{
    [SerializeField] private TweetType m_TweetType;

    private Tweet m_Tweet;

    [SerializeField] private string m_Tag = "nice";

    // Circular Animation Params
    private float m_CircularHeightOffset = 15f;
    private float m_CircularRadius = 250f;
    private float m_CircularFadeInDuration = 0.3f;
    private float m_CircularRisingDuration = 0.5f;
    private float m_CircularWorldSpawnInterval = 0.2f;
    private float m_CircularLifetime = 5f;
    private float m_CircularFadeOutDuration = 2f;
    private float m_CircularSpaceWidth = 0f;

    //[Header("Rising Animation Params")]

    [SerializeField] private TMP_Text m_WordPrefab;

    private void Start()
    {
        var dbTweet = TwitterDatabase.Instance.QueryForTags(m_Tag, 1)[0];
        m_Tweet = new(dbTweet);

        SpawnTweet();
    }

    public void SpawnTweet()
    {
        switch(m_TweetType)
        {
            case TweetType.Circular:
                SpawnCircularTweet();
                break;
            case TweetType.Rising:
                SpawnRisingTweet();
                break;
            default:
                break;
        }
    }

    private void SpawnCircularTweet()
    {
        StartCoroutine(CircularWordAnimation());
    }

    private void SpawnRisingTweet()
    {

    }

    private IEnumerator CircularWordAnimation()
    {
        List<TMP_Text> textObjects = new();
        for (int i = 0; i < m_Tweet.Words.Length; i++)
        {
            TMP_Text text = Instantiate(m_WordPrefab, transform, false);
            text.text = m_Tweet.Words[i];
            //text.alpha = 0f;
            textObjects.Add(text);
        }

        // Wait for one frame
        yield return null;

        // Circular layout
        Vector3 cameraToPointDir = transform.position - Camera.main.transform.position;
        cameraToPointDir.y = 0f;
        cameraToPointDir.Normalize();
        Debug.DrawLine(Camera.main.transform.position, transform.position, Color.blue, 100f);

        float totalWidth = 0f;
        foreach (var textObj in textObjects)
            totalWidth += textObj.preferredWidth + m_CircularSpaceWidth;
        totalWidth -= m_CircularSpaceWidth;

        float totalDegree = totalWidth / (m_CircularRadius * Mathf.PI * 2f) * 360f;
        float accumulatedWidth = 0f;
        for (int i = 0; i < textObjects.Count; i++)
        {
            accumulatedWidth += textObjects[i].preferredWidth / 2f;
            float currentDegree = accumulatedWidth / totalWidth * totalDegree;
            
            Vector3 wordPos = CalculateWordPosition(currentDegree);
            wordPos.y = transform.position.y;
            textObjects[i].transform.position = wordPos;

            //Vector3 pointToWordDir = Quaternion.Euler(0f, currentDegree, 0f) * cameraToPointDir;
            Vector3 pointToWordDir = Quaternion.Euler(0f, -currentDegree, 0f) * Vector3.forward;
            //textObjects[i].transform.forward = pointToWordDir;
            textObjects[i].transform.rotation = Quaternion.LookRotation(pointToWordDir, Vector3.up);

            accumulatedWidth += textObjects[i].preferredWidth / 2f + m_CircularSpaceWidth;
        }
    }

    //private Vector3 CalculateWordPosition(float degree)
    //{
    //    Vector3 cameraPos = Camera.main.transform.position;
    //    float radius = Vector3.Distance(transform.position, cameraPos);
    //    float radians = degree * Mathf.PI / 180f;
    //    float relativeAngle = Mathf.Atan2(transform.position.z - cameraPos.z, transform.position.x - cameraPos.x);
    //    float finalAngle = relativeAngle - radians;

    //    return new Vector3(cameraPos.x + radius * Mathf.Cos(finalAngle), 0f, cameraPos.z + radius * Mathf.Sin(finalAngle));
    //}
    private Vector3 CalculateWordPosition(float degree)
    {
        float radians = degree * Mathf.PI / 180f;
        return new Vector3(transform.position.x + m_CircularRadius * Mathf.Cos(radians), transform.position.y, transform.position.z + m_CircularRadius * Mathf.Sin(radians));
    }
}
