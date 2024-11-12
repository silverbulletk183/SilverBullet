using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RankingScreen : UIScreen
{
    ListView rankingListView;

    public RankingScreen(VisualElement root) : base(root)
    {
    }

    public override void SetVisualElements()
    {
        base.SetVisualElements();
        rankingListView = root.Q<ListView>("listview__ranking");
        LoadData(); // Load data here or call externally when needed
    }

    public override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
    }

    void LoadData()
    {
        // Define your data source as a list of names
        List<string> rankingData = new List<string> { "Quang", "Vinh", "Vi", "Nhi" };

        // Set the items source to the ranking data
        rankingListView.itemsSource = rankingData;

        // Optionally define how each item should appear in the list
        rankingListView.makeItem = () => new Label(); // Creates a Label element for each item
        rankingListView.bindItem = (element, i) =>
        {
            (element as Label).text = rankingData[i]; // Binds data to the Label
            (element as Label).style.color = Color.white;
        };
        
        // Set a fixed item height (optional, for improved layout control)
        rankingListView.fixedItemHeight = 50;
    }
}
