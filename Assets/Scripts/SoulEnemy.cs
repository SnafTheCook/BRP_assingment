using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject InteractionPanelObject;
    [SerializeField] private GameObject ActionsPanelObject;
    [SerializeField] private SpriteRenderer EnemySpriteRenderer;
    [SerializeField] private int BaseSoulReward = 100;
    [SerializeField] private EnemyWeakness Weakness;
    [SerializeField, Range(0.5f, 1.5f)] private float WeaknessSoulMultiplier = 1f;

    private SpawnPoint _enemyPosition;

    public void SetupEnemy(Sprite sprite, SpawnPoint spawnPoint)
    {
        EnemySpriteRenderer.sprite = sprite;
        _enemyPosition = spawnPoint;
        Weakness = (EnemyWeakness)Random.Range(0, System.Enum.GetValues(typeof(EnemyWeakness)).Length); //random ze względu na zadanie
        //normalnie zrobiłym scriptableobject oparty o struct z danymi przeciwników zamiast listy Sprite'ów (EnemiesController.cs)
        gameObject.SetActive(true);
    }

    public SpawnPoint GetEnemyPosition()
    {
        return _enemyPosition;
    }

    public GameObject GetEnemyObject()
    {
        return this.gameObject;
    }

    private void ActiveCombatWithEnemy()
    {
        ActiveInteractionPanel(false);
        ActiveActionPanel(true);
    }

    private void ActiveInteractionPanel(bool active)
    {
        InteractionPanelObject.SetActive(active);
        EventSystem.current.SetSelectedGameObject(ActionsPanelObject.GetComponentInChildren<Selectable>().gameObject);
    }

    private void ActiveActionPanel(bool active)
    {
        ActionsPanelObject.SetActive(active);
    }

    private void UseBow()
    {
        // USE BOW
        GameEvents.EnemyKilled?.Invoke(this);
        GatherWealth(EnemyWeakness.Bow);
    }

    private void UseSword()
    {
        // USE SWORD
        GameEvents.EnemyKilled?.Invoke(this);
        GatherWealth(EnemyWeakness.Sword);
    }

    private void GatherWealth(EnemyWeakness checkWeakness)
    {
        SoulController.Instance.IncrementSouls((int)(BaseSoulReward * (checkWeakness == Weakness ? WeaknessSoulMultiplier : 1)));
    }

    #region OnClicks

    public void Combat_OnClick()
    {
        ActiveCombatWithEnemy();
    }

    public void Bow_OnClick()
    {
        UseBow();
    }

    public void Sword_OnClick()
    {
        UseSword();
    }

    #endregion


    private void OnEnable()
    {
        GameEvents.EnemyKilled += (IEnemy enemy) => EventSystemUtility.SetSelectedFromFirst();
    }

    public enum EnemyWeakness
    {
        None,
        Sword,
        Bow,
    }
}


public interface IEnemy
{
    SpawnPoint GetEnemyPosition();
    GameObject GetEnemyObject();
}

