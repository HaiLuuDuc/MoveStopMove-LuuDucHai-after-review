using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [Header("Position:")]
    public Transform topLeftCorner;
    public Transform bottomRightCorner;
    [SerializeField] private float spawnDistance;
    public float initialY;

    [Header("Manager:")]
    [SerializeField] private Transform bots;
    public BotPool botPool;
    public int botSize;
    public List<Bot> botList = new List<Bot>();

    
    //singleton
    public static BotManager instance;
    private void Awake()
    {
        instance= this;
    }

    void Start()
    {
        SpawnBots();
    }

    public void SpawnBots()
    {
        for(int i = 0; i < botSize; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Bot pooledBot = Cache.GetBot(botPool.GetObject());
        SetPosAndRotFarAwayFromOthers(pooledBot);
        Cache.GetTransform(pooledBot.gameObject).SetParent(bots);
        pooledBot.OnInit();

        SpawnBotPants(pooledBot);
        SpawnBotHat(pooledBot);
        SpawnBotWeapon(pooledBot);
       /* if (pooledBot.botName != null)
        {
            SpawnBotName(pooledBot);
        }*/

        if (botList.Count<botSize)
        {
            botList.Add(pooledBot);
        }
        if (!LevelManager.instance.characterList.Contains(pooledBot))
        {
            LevelManager.instance.characterList.Add(pooledBot);
        }
    }

    public void Despawn(Bot bot)
    {
        bot.DeActiveNavmeshAgent();
        //BotNamePool.instance.ReturnToPool(bot.botName.gameObject);// despawn pooledbotname
        LevelManager.instance.characterList.Remove(bot);
        botPool.ReturnToPool(bot.gameObject);
    }

    public void SpawnNameAndIndicators()//called when PlayGame
    {
        for (int i = 0; i < botList.Count; i++)
        {
            SpawnBotName(botList[i]);
            SpawnBotIndicator(botList[i]);
        }
    }

    public void SpawnBotName(Bot bot)
    {
        //Debug.Log("spawn bot name");
        BotNameUI botName = Cache.GetBotNameUI(BotNamePool.instance.GetObject());
        botName.OnInit();
        botName.SetTargetTransform(Cache.GetTransform(bot.gameObject));
        botName.SetColor(bot);
        bot.botName = botName;
        bot.ActiveName();
    }
   
    public void SpawnBotIndicator(Bot bot) {
        bot.ActiveIndicator();
        bot.indicator.SetColor(bot);
    }
    
    public void DespawnNameAndIndicators()
    {
        for (int i = 0; i < botList.Count; i++)
        {
            DespawnBotName(botList[i]);
            DespawnBotIndicator(botList[i]);
        }
    }

    public void DespawnBotName(Bot bot)
    {
        if(bot.botName != null)
        {
            BotNamePool.instance.ReturnToPool(bot.botName.gameObject);
        }
    }

    public void DespawnBotIndicator(Bot bot)
    {
        if(bot.indicator != null)
        {
            bot.DeactiveIndicator();
        }
    }

    public void SetPosAndRotFarAwayFromOthers(Bot bot)
    {
        Vector3 spawnPosition;
        Vector3 spawnRotation;
        spawnRotation = new Vector3(0, Random.Range(0, 360), 0);
        do
        {
            int randomX = (int)Random.Range(topLeftCorner.position.x, bottomRightCorner.position.x);
            int randomZ = (int)Random.Range(bottomRightCorner.position.z, topLeftCorner.position.z);
            spawnPosition = new Vector3(randomX, initialY, randomZ);
        } while (CheckPositionFarAwayFromOthers(spawnPosition) == false); //spawn position cho bot sao cho nó không đứng gần các thằng khác
        Cache.GetTransform(bot.gameObject).position = spawnPosition;
        Cache.GetTransform(bot.gameObject).rotation = Quaternion.Euler(spawnRotation);
    }
    
    public bool CheckPositionFarAwayFromOthers(Vector3 pos)
    {
        for(int i = 0; i < LevelManager.instance.characterList.Count; i++)
        {
            if (Vector3.Distance(LevelManager.instance.characterList[i].transform.position, pos) < spawnDistance)
            {
                return false;
            }
        }
        return true;
    }

    public void SpawnBotPants(Bot bot)
    {
        int index = (int)Random.Range(0, Inventory.Instance.pants.Length);
        bot.botWearSkinItems.WearPants(index);
    }

    public void SpawnBotHat(Bot bot)
    {
        if (bot.isHaveHat == false)
        {
            int index = (int)Random.Range(0, Inventory.Instance.hats.Length);
            bot.botWearSkinItems.WearHat(index);
            bot.isHaveHat = true;
        }
    }

    public void SpawnBotWeapon(Bot bot)
    {
        if (bot.isHaveWeapon == false)
        {
            int randomWeaponIndex = Random.Range(0, Inventory.Instance.weapons.Length);
            Weapon newWeapon = Cache.GetWeapon(Instantiate(Inventory.Instance.weapons[randomWeaponIndex].gameObject));
            newWeapon.gameObject.SetActive(true);
            Cache.GetTransform(newWeapon.gameObject).SetParent(bot.rightHand);
            Cache.GetTransform(newWeapon.gameObject).localPosition = Vector3.zero;
            Cache.GetTransform(newWeapon.gameObject).localRotation = Quaternion.Euler(Vector3.zero);
            newWeapon.SetOwnerAndWeaponPool(bot, bot.weaponPool);
            bot.onHandWeapon = newWeapon;
            bot.weaponPool.prefab = newWeapon.gameObject;
            Cache.GetBoxCollider(bot.onHandWeapon.gameObject).enabled = false;
            bot.isHaveWeapon = true;
        }
    }

    public void ActiveAllBots()
    {
        for (int i = 0; i < botList.Count; i++)
        {
            if (botList[i].isDead == false)
            {
                botList[i].ChangeState(new PatrolState());
            }
        }
        LevelManager.instance.isGaming = true;
    }

    public void DeactiveAllBots()
    {
        for (int i = 0; i < botList.Count; i++)
        {
            if (botList[i].isDead == false)
            {
                botList[i].ChangeState(new IdleState());
            }
        }
        LevelManager.instance.isGaming = false;
    }

    public bool IsOutOfMap(Vector3 pos)
    {
        if (!(
            pos.x > topLeftCorner.position.x &&
            pos.x < bottomRightCorner.position.x &&
            pos.z > bottomRightCorner.position.z &&
            pos.z < topLeftCorner.position.z
            ))
        {
            return true;
        }
        return false;
    }

   


    public void SpawnBotAfterBotDeath(Bot bot, int alive)
    {
        Despawn(bot);
        if (alive > botSize)
        {
            Spawn();
        }
    }
}
