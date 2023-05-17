using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Player:")]
    [SerializeField] private Player player;

    [Header("targetCircle")]
    [SerializeField] private TargetCircle targetCircle;

    [Header("Lists:")]
    public List<Character> characterList = new List<Character>();

    [Header("Alive:")]
    public int initialAlive;
    public int currentAlive;

    [Header("Bool Variables:")]
    public bool isGaming;

    [Header("Maps:")]
    [SerializeField] private Transform mapParent;
    public GameObject[] mapPrefabs;
    private GameObject currentMap;
    [Header("NavMeshDatas:")]
    public NavMeshData[] navMeshDatas;
    private NavMeshData currentNavMeshData;
    public int currentLevelIndex = 0;

    [Header("Lose info:")]
    public int rank;
    public string enemyName;
    public MaterialType enemyMatType;

    //singleton
    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
        characterList.Add(player);
    }

    void Start()
    {
        BotManager.instance.DeactiveAllBots();
        SpawnMap(currentLevelIndex);
        SpawnNav(currentLevelIndex);
    }

    private void Update()
    {
        if (currentAlive == 1 && player.isDead == false && isGaming == true)
        {
            UIManager.Ins.CloseAll();
            UIManager.Ins.OpenUI<Win>();
            player.Dance();
            PlusLevelIndex();
            DataManager.ins.playerData.currentLevelIndex = this.currentLevelIndex;
            AudioManager.instance.Play(SoundType.Win);
            isGaming = false;
        }
    }

    public void DeleteCharacters()
    {
        if (!player.isDead)
        {
            characterList.Remove(player);
        }
        for(int i=0;i<BotManager.instance.botList.Count;i++)
        {
            // tat het cac weapon dang bay
            Bot bot = BotManager.instance.botList[i];
            for (int j = 0; j < bot.pooledWeaponList.Count; j++)
            {
                if (bot.pooledWeaponList[j].gameObject.activeSelf)
                {
                    bot.pooledWeaponList[j].gameObject.SetActive(false);
                }
            }
            // despawn bot
            BotManager.instance.Despawn(bot);
        }
        characterList.Clear();
    }

    public void RespawnCharacters()
    {
        characterList.Add(player);
        player.OnInit();
        for (int i = 0; i < BotManager.instance.botList.Count; i++)
        {
            BotManager.instance.Spawn();
        }
    }

    public void DeleteCharacterInOtherEnemyLists(Character character)
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].enemyList.Contains(character))
            {
                characterList[i].enemyList.Remove(character);
            }
        }
    }

    public void PlayGame()
    {
        /*DeleteCharacters();
        RespawnCharacters();*/
        BotManager.instance.ActiveAllBots();
        UIManager.Ins.CloseUI<MainMenu>();
        UIManager.Ins.CloseUI<Coin>();
        UIManager.Ins.OpenUI<JoystickUI>();
        UIManager.Ins.OpenUI<GamePlay>();
        currentAlive = initialAlive;
        GamePlay.instance.UpdateAliveTextOnUI();
    }

    public void ResetTargetCircle()
    {
        targetCircle.Deactive();
    }

    public void SpawnMap(int index)
    {
        if(currentMap== mapPrefabs[index])
        {
            return;
        }
        Destroy(currentMap);
        currentMap = Instantiate(mapPrefabs[index]);
        currentMap.transform.SetParent(mapParent.transform);
        currentMap.transform.localPosition = Vector3.zero;
        currentMap.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void SpawnNav(int index)
    {
        if (currentNavMeshData == navMeshDatas[index])
        {
            return;
        }
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(navMeshDatas[index]);
        currentNavMeshData = navMeshDatas[index];
    }

    public void PlusLevelIndex()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= navMeshDatas.Length)
        {
            currentLevelIndex = 0;
        }
    }


}
