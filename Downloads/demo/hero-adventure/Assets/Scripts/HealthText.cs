using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0,75,0);
    public float lifeTime = 1f;
    RectTransform textTransform;
    private float timeElaped=0;
    TextMeshProUGUI text;
    private Color startColor;
    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        text = GetComponent<TextMeshProUGUI>();
        startColor = text.color;
    }

    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;
        timeElaped += Time.deltaTime;
        if (timeElaped < lifeTime)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, startColor.a*( 1 - timeElaped / lifeTime));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
