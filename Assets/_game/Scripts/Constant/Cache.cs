using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Cache
{
    //transform
    private static Dictionary<GameObject, Transform> m_Transform = new Dictionary<GameObject, Transform>();
    public static Transform GetTransform(GameObject key)
    {
        if (!m_Transform.ContainsKey(key))
        {
            m_Transform.Add(key, key.GetComponent<Transform>());
        }

        return m_Transform[key];
    }

    //weapon
    private static Dictionary<GameObject, Weapon> m_Weapon = new Dictionary<GameObject, Weapon>();
    public static Weapon GetWeapon(GameObject key)
    {
        if (!m_Weapon.ContainsKey(key))
        {
            m_Weapon.Add(key, key.GetComponent<Weapon>());
        }

        return m_Weapon[key];
    }

    //bot
    private static Dictionary<GameObject, Bot> m_Bot = new Dictionary<GameObject, Bot>();
    public static Bot GetBot(GameObject key)
    {
        if (!m_Bot.ContainsKey(key))
        {
            m_Bot.Add(key, key.GetComponent<Bot>());
        }

        return m_Bot[key];
    }

    //botnameui
    private static Dictionary<GameObject, BotNameUI> m_BotNameUI = new Dictionary<GameObject, BotNameUI>();
    public static BotNameUI GetBotNameUI(GameObject key)
    {
        if (!m_BotNameUI.ContainsKey(key))
        {
            m_BotNameUI.Add(key, key.GetComponent<BotNameUI>());
        }

        return m_BotNameUI[key];
    }

    //boxcollider
    private static Dictionary<GameObject, BoxCollider> m_BoxCollider = new Dictionary<GameObject, BoxCollider>();
    public static BoxCollider GetBoxCollider(GameObject key)
    {
        if (!m_BoxCollider.ContainsKey(key))
        {
            m_BoxCollider.Add(key, key.GetComponent<BoxCollider>());
        }

        return m_BoxCollider[key];
    }

    //character
    private static Dictionary<GameObject, Character> m_Character = new Dictionary<GameObject, Character>();
    public static Character GetCharacter(GameObject key)
    {
        if (!m_Character.ContainsKey(key))
        {
            m_Character.Add(key, key.GetComponent<Character>());
        }

        return m_Character[key];
    }

}
