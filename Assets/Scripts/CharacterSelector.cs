using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
  public static CharacterSelector instance;
    public CharacterData characterData;
    private void Awake()
    {
        if (instance == null ) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Extra " + this + " Delete!");
            Destroy(gameObject);
        }
    }
    public static CharacterData GetData()
    {
        if (instance && instance.characterData)
        {
            return instance.characterData;
        }
        else
        {
            #if UNITY_EDITOR
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            List<CharacterData> characters = new List<CharacterData>();
            foreach (string asssetPath in allAssetPaths)
            {
                if (asssetPath.EndsWith(".asset"))
                {
                    CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(asssetPath);
                    if (characterData != null)
                    {
                        characters.Add(characterData);
                    }
                }
            }
            // CharacterData[] characters = Resources.FindObjectsOfTypeAll<CharacterData>();
            if (characters.Count > 0)
            {
                return characters[Random.Range(0, characters.Count)];
            }
            #endif

        }
        return null;

    }
    public void SelectCharacter(CharacterData character)
    {
        characterData = character;
    }
    public void DestroySingleton() {
        instance = null;
        Destroy(gameObject);
    }
}
