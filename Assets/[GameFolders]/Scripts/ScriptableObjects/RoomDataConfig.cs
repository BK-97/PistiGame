using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public TableLevels roomType;
    public int minBetValue;
    public int maxBetValue;
}

[CreateAssetMenu(fileName = "RoomDataConfig", menuName = "ScriptableObjects/RoomDataConfig", order = 1)]
public class RoomDataConfig : ScriptableObject
{
    public List<RoomData> roomDatas = new List<RoomData>();
}