using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScoreText : MonoBehaviour
{
    [SerializeField]
    private ScoreScriptableObject scoreObject;
    [SerializeField]
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.SetText($"You survived <b>{scoreObject.lastPhase}</b> phases<br>and destroyed <b>{scoreObject.killCount}</b> enemy drones.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
