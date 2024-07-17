using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MapData;

public class MapController:MonoBehaviour
    {
    public MapData listMaps;
    public TMP_Text nameMap;
    public SpriteRenderer mapPic;
    private int selectedIndex = 0;
    private MapData.Map map;

    private void Start()
    {
        UpdateMapDisplay();

    }
    private void UpdateMapDisplay()
    {
       map = listMaps.getMap(selectedIndex);
        nameMap.text = map.mapName;
        mapPic.sprite = map.mapPrefab;
    }
     
    public void Play()
    {
        SceneController sc = FindObjectOfType<SceneController>();
        Debug.Log(sc == null);
        Debug.Log(map.mapStageToChange);

        sc.SceneChange(map.mapStageToChange);
    }

    public void NextMapOptions()
    {
        Debug.LogWarning(listMaps.MapCount);
        Debug.LogWarning(selectedIndex);

        selectedIndex++;
        if (selectedIndex>=listMaps.MapCount)
        {
            selectedIndex = 0;
        }
        UpdateMapDisplay();
    }
    public void BackMapOptions()
    {
        selectedIndex--;
        if (selectedIndex < 0 )
        {
            selectedIndex = listMaps.MapCount - 1;
        }
        UpdateMapDisplay();
    }

}

