using UnityEngine;

[CreateAssetMenu(fileName = "ReactionTimeContainerTemplate", menuName = "Scriptable Objects/ReactionTimeContainerTemplate")]
public class ReactionTimeContainerTemplate : ScriptableObject
{
    public int _misses = 0;
    public int _hits = 0;
    public double _cumulativeReactionTimeMS = 0.0f;

    public void OnEnable()
    {
        _misses = 0;
        _hits = 0;
        _cumulativeReactionTimeMS = 0.0f;
    }

    /*public void SetData(float reactionTime, HitType type)
    {
        switch (type)
        {
            case HitType.HIT:
                _hits++;
                break;
            case HitType.MISS:
                _misses++;
                break;
            default:
                break;
        }

        _cumulativeReactionTime += reactionTime;

    }*/
}
