using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class NameUI : MonoBehaviour
{
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected TextMeshProUGUI tmp;
    [SerializeField] protected Vector3 offset;
    [SerializeField] protected float speed;
    protected Vector3 targetPosition;
    public string nameString;

    public virtual void OnInit() { }

    public virtual void LateUpdate()
    {
        targetPosition = Camera.main.WorldToScreenPoint(targetTransform.position + offset);
        rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, speed * Time.deltaTime);
    }

    public virtual void SetTargetTransform(Transform targetTF)
    {
        targetTransform = targetTF;
    }

    public virtual void SetColor(Bot bot)
    {
        tmp.color = bot.skinnedMeshRenderer.material.color;
    }

    public virtual void SetNameUI(string name)
    {
        tmp.text = name;
    }
}
