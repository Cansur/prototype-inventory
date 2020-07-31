using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Item
{
    public Item(string _Rank, string _Type, string _Name, string _Explain, string _Number, bool _isUsing)
    { Rank = _Rank; Type = _Type; Name = _Name; Explain = _Explain; Number = _Number; isUsing = _isUsing; }

    public string Rank, Type, Name, Explain, Number;
    public bool isUsing;
}

public class Inventory : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList, CurItemList;
    public string curType = "Skill";
    public GameObject[] Slot;
    public Image[] ItemImage;
    public Sprite[] ItemSprite;
    GameObject SlotBackGround;
    GameObject SlotPanel;
    //[SerializeField] GameObject Slottransform;
    string filePath; // C:/Users/USER/AppData/LocalLow/DefaultCompany/New Unity Project

    void Start()
    {
        StartCaching();

        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4], row[5] == "TRUE"));
        }
        
        //Save();
        Load();
        
        SlotBackGroundStartFalse();
        //Slot = Resources.Load<GameObject>("Slot");
        //Slottransform = GameObject.FindWithTag("Inventory");
        //SlotObject(5);
    }
    
    public void SlotClick(int slotNum)
    {
        Item CurItem = CurItemList[slotNum];
        Item UsingItem = CurItemList.Find(x => x.isUsing == true);
        SlotBackGround.SetActive(true);

        SlotPanel.transform.GetChild(0).GetComponent<Image>().sprite = Slot[slotNum].GetComponent<Image>().sprite;
        SlotPanel.transform.GetChild(1).GetComponent<Text>().text = CurItemList[slotNum].Rank + "Rank";
        SlotPanel.transform.GetChild(2).GetComponent<Text>().text = CurItemList[slotNum].Name;
    }

    public void TabClick(string tabName)
    {
        curType = tabName;
        CurItemList = MyItemList.FindAll(x => x.Type == tabName);

        for (int i = 0; i < Slot.Length; i++)
        {
            //Slot[i].SetActive(i < CurItemList.Count);
            bool isExit = i < CurItemList.Count;
            Slot[i].SetActive(isExit);

            if (isExit)
            {
                ItemImage[i].sprite = ItemSprite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
            }
        }
    }

    public void SlotExitButton()
    {
        SlotBackGround.SetActive(false);
    }

    void StartCaching()
    {
        SlotBackGround = GameObject.FindWithTag("Canvas").transform.Find("SlotBackGround").gameObject;
        SlotPanel = SlotBackGround.transform.Find("SlotPanel").gameObject;

        ItemDatabase = Resources.Load<TextAsset>("SkillBase");

        filePath = Application.persistentDataPath + "/MyItemText.txt";
    }

    void Save()
    {
        string jdata = JsonConvert.SerializeObject(AllItemList);
        File.WriteAllText(filePath, jdata);

        TabClick(curType);
    }

    void Load()
    {
        string jdata = File.ReadAllText(filePath);
        MyItemList = JsonConvert.DeserializeObject<List<Item>>(jdata);

        TabClick(curType);
    }

    void SlotObject(int Number)
    {
        for (int i = 0; i < Number; i++)
        {
            //Slottransform.transform.parent
        }
    }

    void SlotSelect()
    {
        
    }

    void SlotBackGroundStartFalse()
    {
        if (SlotBackGround.gameObject.activeSelf)
        {
            SlotBackGround.SetActive(false);
        }
    }
    
    
}
