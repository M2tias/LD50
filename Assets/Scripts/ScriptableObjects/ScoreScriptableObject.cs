using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ScoreObject", menuName = "Configs/ScoreObject", order = 1)]
public class ScoreScriptableObject : ScriptableObject
{
    public int lastPhase;
    public int lastTier;
    public int killCount;
}
