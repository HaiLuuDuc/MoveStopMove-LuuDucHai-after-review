using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Player player;
    public Text timerText;

    public void OnEnable()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(TimerCount());
    }

    public IEnumerator TimerCount()
    {
        float t = duration;
        while (t > 0)
        {
            t -= Time.deltaTime;
            timerText.text = ((int)t).ToString();
            yield return null;
        }
        UIManager.Ins.CloseUI<Revive>();
        UIManager.Ins.OpenUI<Lose>();
        LevelManager.instance.DeleteCharacterInOtherEnemyLists(player);
        LevelManager.instance.currentAlive--;
        GamePlay.instance.UpdateAliveTextOnUI();
        LevelManager.instance.characterList.Remove(player);
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play(SoundType.Lose);
        }
        yield return null;
    }
}
