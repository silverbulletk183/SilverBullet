using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData/Levels/LevelData", menuName = "SilverBullet/Level")]
public class LevelSO : ScriptableObject
{
    public int levelNumber;
    public string levelLabel;
    public Sprite thumbnail;
    public string sceneName;

}
