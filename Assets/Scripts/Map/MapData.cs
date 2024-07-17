using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Data", menuName = "Data/MapData")]
public class MapData : ScriptableObject
{
    [System.Serializable]
    public class Map
    {
        public string mapName;
        public string mapStageToChange;
        public Sprite mapPrefab;
    }

    public Map[] listMaps;

    public int MapCount
    {
        get { return listMaps.Length;}
    }
    public Map getMap(int index)
    {
        return listMaps[index]; 
    }
}

