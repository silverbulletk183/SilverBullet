using System.Collections.Generic;

[System.Serializable]

public class Gun
{
    public string name;
    public string type;
    public int damage;
    public int rateOfFire;
    public int range;
    public int magazineSize;
    public float reloadTime;
    public int accuracy;
    public Recoil recoil;
    public string ammoType;
}

[System.Serializable]
public class Recoil
{
    public float horizontal;
    public float vertical;
}

[System.Serializable]
public class GunList
{
    public List<Gun> guns;
}