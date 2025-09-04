using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryView : UiView
{
    [Header("Inventory Elements")] [SerializeField]
    private SoulInformation SoulItemPlaceHolder;

    [SerializeField] private Text Description;
    [SerializeField] private Text Name;
    [SerializeField] private Image Avatar;
    [SerializeField] private Button UseButton;
    [SerializeField] private Button DestroyButton;
    [SerializeField] private GridLayoutGroup GridLayout;

    private RectTransform _contentParent;
    private GameObject _currentSelectedGameObject;
    private SoulInformation _currentSoulInformation;

    private List<Button> _soulButtons = new List<Button>();

    public override void Awake()
    {
        base.Awake();
        _contentParent = (RectTransform)SoulItemPlaceHolder.transform.parent;
        InitializeInventoryItems();
    }

    private void Start()
    {
        SetInventoryItemsNeighbours();
        SetFirstSelectedByEventSystem(_soulButtons[0].gameObject);
    }

    private void InitializeInventoryItems()
    {
        for (int i = 0, j = SoulController.Instance.Souls.Count; i < j; i++)
        {
            SoulInformation newSoul = Instantiate(SoulItemPlaceHolder.gameObject, _contentParent).GetComponent<SoulInformation>();
            newSoul.SetSoulItem(SoulController.Instance.Souls[i], () => SoulItem_OnClick(newSoul));

            _soulButtons.Add(newSoul.GetComponent<Button>());
        }

        SoulItemPlaceHolder.gameObject.SetActive(false);
    }

    private void SetInventoryItemsNeighbours()
    {
        int columnSize = GridLayout.constraintCount; //cache dla uproszczenia
        
        for (int i = 0; i < _soulButtons.Count; i++)
        {
            _soulButtons[i].navigation = new NavigationBuilder()
                .WithRightNeighbour(i % columnSize != columnSize - 1  && _soulButtons.Count - 1 > i ? _soulButtons[i + 1] : GetClosestButtonOnRight())
                .WithLeftNeighbour(i % columnSize != 0 ? _soulButtons[i - 1] : null)
                .WithUpNeighbour(i - columnSize < 0 ? null : _soulButtons[i - columnSize])
                .WithDownNeighbour(_soulButtons.Count - 1 >= i + columnSize ? _soulButtons[i + columnSize] : null)
                .Build();
        }
    }

    private Button GetClosestButtonOnRight()
    {
        Button button = null;
        if (UseButton.isActiveAndEnabled && UseButton.interactable) button = UseButton;
        else if (DestroyButton.isActiveAndEnabled && DestroyButton.interactable) button = DestroyButton;
        else button = BackButon;

        return button;
    }

    public override void OnEnable()
    {
        ClearSoulInformation();
        base.OnEnable();
    }

    private void ClearSoulInformation()
    {
        Description.text = "";
        Name.text = "";
        Avatar.sprite = null;
        SetupUseButton(false);
        SetupDestroyButton(false);
        _currentSelectedGameObject = null;
        _currentSoulInformation = null;

        _soulButtons.RemoveAll(item => item == null || !item.gameObject.activeInHierarchy);

        SetInventoryItemsNeighbours();
        SetFirstSelectedByEventSystem(_soulButtons[0].gameObject);
    }

    public void SoulItem_OnClick(SoulInformation soulInformation)
    {
        _currentSoulInformation = soulInformation;
        _currentSelectedGameObject = soulInformation.gameObject;
        SetupSoulInformation(soulInformation.soulItem);
        SetInventoryItemsNeighbours();
    }

    private void SetupSoulInformation(SoulItem soulItem)
    {
        Description.text = soulItem.Description;
        Name.text = soulItem.Name;
        Avatar.sprite = soulItem.Avatar;
        SetupUseButton(soulItem.CanBeUsed);
        SetupDestroyButton(soulItem.CanBeDestroyed);
    }

    private void SelectElement(int index)
    {

    }

    private void CantUseCurrentSoul()
    {
        PopUpInformation popUpInfo = new PopUpInformation { DisableOnConfirm = true, UseOneButton = true, Header = "CAN'T USE", Message = "THIS SOUL CANNOT BE USED IN THIS LOCALIZATION" };
        GUIController.Instance.ShowPopUpMessage(popUpInfo);
    }

    private void UseCurrentSoul(bool canUse)
    {
        if (!canUse)
        {
            CantUseCurrentSoul();
        }
        else
        {
            //USE SOUL
            SoulController.Instance.IncrementSouls(_currentSoulInformation.soulItem.SoulWorth);
            Debug.Log(_currentSelectedGameObject.name);
            DestroyImmediate(_currentSelectedGameObject);
            ClearSoulInformation();
        }
    }

    private void DestroyCurrentSoul()
    {
        DestroyImmediate(_currentSelectedGameObject);
        ClearSoulInformation();
    }

    private void SetupUseButton(bool active)
    {
        UseButton.onClick.RemoveAllListeners();
        if (active)
        {
            bool isInCorrectLocalization = GameControlller.Instance.IsCurrentLocalization(_currentSoulInformation.soulItem.UsableInLocalization);
            PopUpInformation popUpInfo = new PopUpInformation
            {
                DisableOnConfirm = isInCorrectLocalization,
                UseOneButton = false,
                Header = "USE ITEM",
                Message = "Are you sure you want to USE: " + _currentSoulInformation.soulItem.Name + " ?",
                Confirm_OnClick = () => UseCurrentSoul(isInCorrectLocalization)
            };
            UseButton.onClick.AddListener(() => GUIController.Instance.ShowPopUpMessage(popUpInfo));
            UseButton.interactable = isInCorrectLocalization;
        }
        UseButton.gameObject.SetActive(active);
    }

    private void SetupDestroyButton(bool active)
    {
        DestroyButton.onClick.RemoveAllListeners();
        if (active)
        {
            PopUpInformation popUpInfo = new PopUpInformation
            {
                DisableOnConfirm = true,
                UseOneButton = false,
                Header = "DESTROY ITEM",
                Message = "Are you sure you want to DESTROY: " + Name.text + " ?",
                Confirm_OnClick = () => DestroyCurrentSoul()
            };
            DestroyButton.onClick.AddListener(() => GUIController.Instance.ShowPopUpMessage(popUpInfo));
        }

        DestroyButton.gameObject.SetActive(active);
    }
}