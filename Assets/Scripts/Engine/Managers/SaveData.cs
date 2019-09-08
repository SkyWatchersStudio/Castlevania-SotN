using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int experience, playerLevel, nextLevelPoint, money;
    public float[] position = new float[3];
    public int saveRoomIndex;

    public SaveData(int xp, int plLevel, int nextLevelPoint,
                    int coins, Vector3 pos, int roomIndex)
    {
        this.experience = xp;
        this.playerLevel = plLevel;
        this.nextLevelPoint = nextLevelPoint;
        this.money = coins;
        this.saveRoomIndex = roomIndex;

        for (int i = 0; i < 3; i++)
            this.position[i] = pos[i];
    }
}
