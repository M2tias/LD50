using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [SerializeField]
    private GameObject lootTextPrefab;
    [SerializeField]
    private Transform lootTextParent;
    [SerializeField]
    private TextMeshProUGUI bottleText;
    [SerializeField]
    private TextMeshProUGUI cogText;
    [SerializeField]
    private TextMeshProUGUI rocketText;
    [SerializeField]
    private TextMeshProUGUI oilcanText;
    [SerializeField]
    private TextMeshProUGUI telescopeText;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI currentPhaseText;
    [SerializeField]
    private Image hpImage;

    private int bottles = 0;
    private int cogs = 0;
    private int rockets = 0;
    private int telescopes = 0;
    private int oilcans = 0;

    void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        updateCounterText(bottles, bottleText);
        updateCounterText(bottles, cogText);
        updateCounterText(bottles, rocketText);
        updateCounterText(bottles, telescopeText);
        updateCounterText(bottles, oilcanText);
    }

    // Update is called once per frame
    void Update()
    {
        updateCurrentPhase();
    }

    public void ShowLootText(string text)
    {
        GameObject obj = Instantiate(lootTextPrefab);
        obj.transform.SetParent(lootTextParent, false);
        LootText t = obj.GetComponent<LootText>();
        t.Initialize(text);
    }

    // Update HP
    public void UpdateHP()
    {
        float hp = GameManager.main.GetCurrentHP();
        float max = GameManager.main.GetMaxHP();
        updateHPText(hp, max);
        float hpBarWidth = Mathf.Clamp(200 * (hp / max), 0, 200);
        hpImage.GetComponent<RectTransform>().sizeDelta = new Vector2(hpBarWidth, 40);
    }

    public void UpdatePickedUpLootCounter(LootType type)
    {
        switch (type)
        {
            case LootType.Bottle:
                bottles++;
                updateCounterText(bottles, bottleText);
                break;
            case LootType.Cog:
                cogs++;
                updateCounterText(cogs, cogText);
                updateHPText(GameManager.main.GetCurrentHP(), GameManager.main.GetMaxHP());
                break;
            case LootType.Rocket:
                rockets++;
                updateCounterText(rockets, rocketText);
                break;
            case LootType.Telescope:
                telescopes++;
                updateCounterText(telescopes, telescopeText);
                break;
            case LootType.OilCan:
                oilcans++;
                updateCounterText(oilcans, oilcanText);
                break;
            default:
                break;
        }
    }

    private void updateCounterText(int value, TextMeshProUGUI text)
    {
        text.SetText($"x {value}");
    }

    private void updateHPText(float currentHP, float maxHP)
    {
        hpText.SetText($"{(int)currentHP} / {(int)maxHP}");
    }

    private void updateCurrentPhase()
    {
        currentPhaseText.SetText($"Phase {GameManager.main.GetCurrentPhase() + 1}");
    }
}
