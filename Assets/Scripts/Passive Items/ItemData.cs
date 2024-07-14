using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public Sprite icon;
    public int maxLevel;


    [System.Serializable]
    public struct Evolution
    {
        public string name;
        public enum Condition { auto ,anvil }
        public Condition condition; // anvil nghia la dung vao cai de -> tien hoa 
      

        [System.Flags] public enum Consumption { weapon = 1, passive = 2 }
        public Consumption consumes; // de tao list hoac xoa weapon, xoa passive hoac ca 2
        public int evolutionLevel; // cap tien hoa == max level cua item can tien hoa
        public Config[] catalysts; // danh sach nhung trang bi can hien te va dieu kien
        public Config outcome; // NEU EVOLUTION thi no se evolution thanh cai nay

        // auto la khi du dieu kien, catalyst du thi tien hoa

        [System.Serializable]
        public struct Config //cau truc Nguyen lieu hien te va Thanh pham
        {
            public ItemData itemType; // loai scriptable object , passiveData, weaponData
            public int level; // cap can co
        }
    }
    public Evolution[] evolutionData;

    public abstract Item.LevelData GetLevelData(int level);
}
