using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LootText : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private float fadeSpeed;

    private RectTransform rectT;
    private TextMeshProUGUI mText;
    private Color startColor;
    private Color endColor;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        rectT = this.GetComponent<RectTransform>();
        startColor = mText.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
    }

    // nice color: #FF7777
    public void Initialize(string text)
    {
        Debug.Log("Loot text initialized");
        //  startColor = c;
        mText = gameObject.GetComponent<TextMeshProUGUI>();
        mText.SetText(text);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = rectT.localPosition;
        rectT.localPosition = new Vector3(0, v.y + scrollSpeed * Time.deltaTime, v.z);
        mText.color = Color.Lerp(startColor, endColor, t);
        t = t + fadeSpeed * Time.deltaTime;
    }
}
