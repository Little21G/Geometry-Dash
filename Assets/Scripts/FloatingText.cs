using UnityEngine;
using TMPro; // Needed for TextMeshPro

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f; // How fast it floats up
    public float fadeSpeed = 1.5f; // How fast it turns invisible

    private TextMeshProUGUI textMesh;
    private Color textColor;

    public void Setup(int bonusAmount)
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + bonusAmount.ToString();
        textColor = textMesh.color;
    }

    void Update()
    {
        // 1. Float upward
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // 2. Fade transparency (Alpha)
        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;

        // 3. Destroy it when it is completely invisible
        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}