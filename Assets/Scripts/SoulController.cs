using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    private static SoulController _instance;
    public static SoulController Instance
    {
        get
        {
            if (_instance == null) _instance = FindFirstObjectByType<SoulController>();
            return _instance;
        }
        set => _instance = value;
    }
    public List<SoulItem> Souls;

    private int _soulsScore = 0;
    private IScoreViewable _scoreView;

    private void Awake()
    {
        Instance = this;
        _scoreView = GetComponent<IScoreViewable>();

        UpdateSouls();
    }

    public void IncrementSouls(int incrementAmount)
    {
        _soulsScore += incrementAmount;
        UpdateSouls();
    }

    private void UpdateSouls()
    {
        _scoreView?.UpdateScore(_soulsScore);
    }
}
