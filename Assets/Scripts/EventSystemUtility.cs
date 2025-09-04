using UnityEngine;
using UnityEngine.EventSystems;

public static class EventSystemUtility
{
    public static void SetSelectedFromFirst()
    {
        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }
}
