using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float lifetime = 1f;
    public float floatSpeed = 40f;

    private TextMeshProUGUI text;
    private Transform target;
    private float stackOffset;
    private Vector3 baseOffset;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 worldPos = target.position + baseOffset + new Vector3(0, stackOffset, 0);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            transform.position = screenPos;
        }

        stackOffset += Time.deltaTime * 1.0f;
    }

    public void SetText(string value)
    {
        if (text != null)
            text.text = value;
    }

    public void SetTarget(Transform t, float offset)
    {
        target = t;
        baseOffset = new Vector3(0, 2f, 0);
        stackOffset = offset;
    }
}
