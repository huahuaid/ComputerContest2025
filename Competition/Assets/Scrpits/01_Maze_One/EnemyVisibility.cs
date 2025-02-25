using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    [Header("视觉效果")]
    public Material visibleMaterial;
    public Material hiddenMaterial;
    [Range(0.1f, 5f)] public float fadeSpeed = 2f;

    private Renderer enemyRenderer;
    private float currentAlpha = 0f;
    private bool isVisible = false;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        enemyRenderer.material = hiddenMaterial;
        UpdateMaterialAlpha(0f);
    }

    void Update()
    {
        HandleVisibilityTransition();
    }

    public void SetVisible(bool visible)
    {
        isVisible = visible;
    }

    void HandleVisibilityTransition()
    {
        float targetAlpha = isVisible ? 1f : 0f;
        currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        UpdateMaterialAlpha(currentAlpha);
    }

    void UpdateMaterialAlpha(float alpha)
    {
        Color color = enemyRenderer.material.color;
        color.a = alpha;
        enemyRenderer.material.color = color;
        
        enemyRenderer.material = alpha > 0.1f ? visibleMaterial : hiddenMaterial;
    }
}
