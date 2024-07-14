using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
[RequireComponent(typeof(VerticalLayoutGroup))]
public class UIUpgradeWindow : MonoBehaviour
{
    VerticalLayoutGroup verticalLayout;

    // The button and tooltip template GameObjects we have to assign.

    public RectTransform upgradeOptionTemplate;
    public TextMeshProUGUI tooltipTemplate;

    [Header("Settings")]
    public int maxOptions = 4;
    public string newtext = "New!"; // The text that shows when a nav apgrade is shown,

    public Color newTextColor = Color.yellow, levelTextColor = Color.white;

    // These are the paths to the different it elments in the upgradeOptionTemplate>.

    [Header("Paths")]
    public string iconPath = "Icon/Item Icon";
    public string namePath = "Name",
        descriptionPath = "Description",
        buttonPath = "Button",
        levelPath = "Level";

    RectTransform rectTransform;
    float optionHeight;
    int activeOptions; // Tracks the number of options that are active currently.

    // This is a list of all the upgrade buttons on the window.

    List<RectTransform> upgradeOptions = new List<RectTransform>();

    Vector2 lastScreen;


    public void SetUpgrades(PlayerInventory inventory, List<ItemData> possibleUpgrades, int pick = 3, string tooltip = "")
    {
        pick = Mathf.Min(maxOptions, pick);

        // If we don't have enough upgrade option boxes, create then.

        if (maxOptions > upgradeOptions.Count)
        {
            for (int i = upgradeOptions.Count; i < pick; i++)
            {

                GameObject go = Instantiate(upgradeOptionTemplate.gameObject, transform);
                upgradeOptions.Add((RectTransform)go.transform);
            }
        }
        // If a string is provided, turn on the tooltip.

        tooltipTemplate.text = tooltip;

        tooltipTemplate.gameObject.SetActive(tooltip.Trim() != "");

        activeOptions = 0;

        int totalPossibleUpgrades = possibleUpgrades.Count; // How many upgrades do we have to choose fros
        foreach (RectTransform r in upgradeOptions)
        {
            if (activeOptions < pick && activeOptions < totalPossibleUpgrades)
            {
                r.gameObject.SetActive(true);

                // Select one of the possible upgrades, then remove it from the list.
                ItemData selected = possibleUpgrades[UnityEngine.Random.Range(0, possibleUpgrades.Count)];
                possibleUpgrades.Remove(selected);
                Item item = inventory.Get(selected);

                TextMeshProUGUI name = r.Find(namePath).GetComponent<TextMeshProUGUI>();
                if (name)
                {
                    name.text = selected.name;
                }

                TextMeshProUGUI level = r.Find(levelPath).GetComponent<TextMeshProUGUI>();
                if (level)
                {
                    if (item)
                    {
                        if (item.currentLevel > item.maxLevel)
                        {
                            level.text = "Max!";
                            level.color = newTextColor;
                        }
                        else
                        {
                            level.text = selected.GetLevelData(item.currentLevel).name;
                            level.color = levelTextColor;
                        }
                    }
                }
                TextMeshProUGUI desc = r.Find(descriptionPath).GetComponent<TextMeshProUGUI>();
                if (desc)
                {
                    if (item)
                    {
                        desc.text = selected.GetLevelData(item.currentLevel + 1).description;
                    }
                    else
                    {
                        desc.text = selected.GetLevelData(1).description;
                    }
                }


                Image icon = r.Find(iconPath).GetComponent<Image>();
                if (icon)
                {
                    icon.sprite = selected.icon;
                }



                Button b = r.Find(buttonPath).GetComponent<Button>();
                if (b)
                {
                    b.onClick.RemoveAllListeners();
                    if (item)
                    {
                        b.onClick.AddListener(() => inventory.LevelUp(item));
                    }
                    else
                    {
                        b.onClick.AddListener(() => inventory.Add(selected));
                    }
                }

                activeOptions++;
            }

            else r.gameObject.SetActive(false);
        }
        RecalculateLayout();
    }

    void RecalculateLayout()   {
        // Calculates the total available height for all options, then divides it by the number of options.
        rectTransform = GetComponent<RectTransform>();
        verticalLayout = GetComponent<VerticalLayoutGroup>();
      //  optionHeight = (rectTransform.rect.height - verticalLayout.padding.top - verticalLayout.padding.bottom - (maxOptions - 1) * verticalLayout.spacing);
      optionHeight = 288f;
        if (activeOptions == maxOptions && tooltipTemplate.gameObject.activeSelf)
            optionHeight /= maxOptions + 1;
        else
            optionHeight /= maxOptions;

        //Recalculates the height of the tooltip as well if it is currently active.
        if (tooltipTemplate.gameObject.activeSelf)
        {
            RectTransform tooltipRect = (RectTransform)tooltipTemplate.transform;
            tooltipTemplate.gameObject.SetActive(true);
            tooltipRect.sizeDelta = new Vector2(tooltipRect.sizeDelta.x, optionHeight);
            tooltipTemplate.transform.SetAsLastSibling();

            // Sets the height of every active Upgrade Option button.


        }
        foreach (RectTransform r in upgradeOptions)
        {
            if (!r.gameObject.activeSelf) continue;
            r.sizeDelta = new Vector2(r.sizeDelta.x, optionHeight);
        }
    }

    void Update()
    {
        if (lastScreen.x != Screen.width || lastScreen.y != Screen.height)
        {
            RecalculateLayout();
            lastScreen = new Vector2(Screen.width, Screen.height);
        }
    }
    void Awake()

    {
        // Populates all our important variables.
        verticalLayout = GetComponentInChildren<VerticalLayoutGroup>();
        if (tooltipTemplate) tooltipTemplate.gameObject.SetActive(false);
        if (upgradeOptionTemplate) upgradeOptions.Add(upgradeOptionTemplate);
        // Get the RectTransform of this object for height calculations.
        rectTransform = (RectTransform)transform;
    }
    void Reset()
    {
        upgradeOptionTemplate = (RectTransform)transform.Find("Upgrade Option");
        tooltipTemplate = transform.Find("Tooltip").GetComponent<TextMeshProUGUI>();
    }
}
