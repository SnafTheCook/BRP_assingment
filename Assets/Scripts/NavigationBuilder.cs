using UnityEngine;
using UnityEngine.UI;

public class NavigationBuilder
{
    private MyNavigation myNavi;

    public NavigationBuilder()
    {
        myNavi = new MyNavigation();
    }

    public NavigationBuilder WithLeftNeighbour(Selectable leftNeighbour)
    {
        myNavi.leftNeighbour = leftNeighbour;
        return this;
    }

    public NavigationBuilder WithRightNeighbour(Selectable rightNeighbour)
    {
        myNavi.rightNeighbour = rightNeighbour;
        return this;
    }

    public NavigationBuilder WithUpNeighbour(Selectable upNeighbour)
    {
        myNavi.upNeighbour = upNeighbour;
        return this;
    }

    public NavigationBuilder WithDownNeighbour(Selectable downNeighbour)
    {
        myNavi.downNeighbour = downNeighbour;
        return this;
    }

    public Navigation Build()
    {
        Navigation nav = new();
        nav.mode = Navigation.Mode.Explicit;

        nav.selectOnDown = myNavi.downNeighbour;
        nav.selectOnUp = myNavi.upNeighbour;
        nav.selectOnRight = myNavi.rightNeighbour;
        nav.selectOnLeft = myNavi.leftNeighbour;

        return nav;
    }
}

public class MyNavigation
{
    public Selectable leftNeighbour;
    public Selectable rightNeighbour;
    public Selectable upNeighbour;
    public Selectable downNeighbour;
}
