using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUI : MonoBehaviour
{
    // Start is called before the first frame update
    public static GunUI Instance { get; private set; }
    [SerializeField] private GameObject gunItem;
    [SerializeField] private Transform content;
    private void Awake()
    {
        Instance = this;
    }
    public void PopulateShop(List<Gun> guns)
    {
        //Debug.Log("listcharacter " + characters.Count);
        foreach (var gun in guns)
        {
            GameObject item = Instantiate(gunItem, content);
            GunItemUI ui = item.GetComponent<GunItemUI>();
            if (ui != null)
            {
                Debug.Log("set characterdata" + gun);
                ui.SetupGunData(gun);
                ui.checkAlreadyBought();
            }
        }
        ShopUI.Instance.ShowLoadingUI(false);
    }
}
