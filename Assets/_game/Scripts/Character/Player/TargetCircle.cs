using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TargetCircle : MonoBehaviour
{

    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private float rotateSpeed;
    public Transform enemyTransform;

    void Update()
    {
        if(this.gameObject.activeSelf)
        {
            Cache.GetTransform(this.gameObject).Rotate(0, rotateSpeed, 0);
            if(enemyTransform != null)
            {
                Cache.GetTransform(this.gameObject).position = enemyTransform.position;
            }
        }
    }
    
    public void Active()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        if (playerAttack.enemy != null)
        {
            this.enemyTransform = Cache.GetTransform(playerAttack.enemy.gameObject);
        }
    }

    public void Deactive()
    {
        if (this.gameObject.activeSelf)
        {
            Cache.GetTransform(this.gameObject).position -= new Vector3(0, 100, 0); // fix loi targetCircle nhap nhay o enemy cu~
            this.gameObject.SetActive(false);
        }
    }
}
