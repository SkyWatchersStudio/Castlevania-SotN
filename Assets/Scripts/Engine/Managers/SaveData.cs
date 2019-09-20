using UnityEngine;

[System.Serializable]
public struct SaveData
{
    public int experience, playerLevel, nextLevelPoint, money;
    public float[] position;
    public int saveRoomIndex;

    public float damage, maxHealth, maxMana;
    public int hearts, maxHearts;

    public int potions;
    public bool fireSword, iceSword;

    public bool cubeOfZoe, mist, dash;
}
