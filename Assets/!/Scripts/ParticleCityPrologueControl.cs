using UnityEngine;
using System.Collections.Generic;

public class ParticleCityPrologueControl : MonoBehaviour
{
    [SerializeField] private Texture2D m_BaseTexture;

    [SerializeField] private Texture2D m_SparkleTexture;

    [Range(0, 1)]
    [SerializeField] private float m_SparkleRatio = 0.2f;

    [SerializeField] private Vector2 m_SparkleIntensityRange = new(1.4f, 3f);

    [SerializeField] private Vector2 m_SparkleSizeRange = new(1.5f, 4f);

    [SerializeField] private float m_IncreaseDuration = 5f;

    [SerializeField] private float m_DecreaseDuration = 5f;

    [SerializeField] private float m_IntervalDuration = 4f;

    private List<MeshRenderer> m_BaseMeshRenderers = new();

    private List<MeshRenderer> m_SparkleMeshRenderers = new();

    private void Start()
    {
        var meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (var meshRenderer in meshRenderers)
        {
            float random = Random.Range(0f, 1f);
            if (random < m_SparkleRatio)
                m_SparkleMeshRenderers.Add(meshRenderer);
            else
                m_BaseMeshRenderers.Add(meshRenderer);
        }

        foreach (var meshRenderer in m_BaseMeshRenderers)
        {
            meshRenderer.material.SetTexture("_ColorPalleteTex", m_BaseTexture);
            meshRenderer.gameObject.name = "Base";
        }

        foreach (var meshRenderer in m_SparkleMeshRenderers)
        {
            meshRenderer.material.SetTexture("_ColorPalleteTex", m_SparkleTexture);
            meshRenderer.gameObject.name = "Sparkle";
        }

        OnInterval();
    }

    private void OnIncrease()
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, m_SparkleSizeRange.x, m_SparkleSizeRange.y, m_IncreaseDuration)
            .setOnUpdate((float size) =>
            {
                foreach (var meshRenderer in m_SparkleMeshRenderers)
                    meshRenderer.material.SetFloat("_Size", size);
            })
            .setOnComplete(() =>
            {
                OnDecrease();
            });
    }

    private void OnDecrease()
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, m_SparkleSizeRange.y, m_SparkleSizeRange.x, m_DecreaseDuration)
            .setOnUpdate((float size) =>
            {
                foreach (var meshRenderer in m_SparkleMeshRenderers)
                    meshRenderer.material.SetFloat("_Size", size);
            })
            .setOnComplete(() =>
            {
                OnInterval();
            });
    }

    private void OnInterval()
    {
        LeanTween.cancel(gameObject);
        LeanTween.delayedCall(m_IntervalDuration, () =>
        {
            OnIncrease();
        });
    }
}
