using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour, IScoreViewable
{
    [SerializeField] private Text soulsText;

    public void UpdateScore(int newScore)
    {
        soulsText.text = newScore.ToString();
    }
}

public interface IScoreViewable
{
    void UpdateScore(int newScore);
}
