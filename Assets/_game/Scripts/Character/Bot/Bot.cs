using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Bot : Character, IChangeState, INavMeshAgent
{
    [Header("AI:")]
    [Header("Bot class:")]
    public Transform destinationTransform;
    public NavMeshAgent navMeshAgent;
    private Transform centerOfMap;
    [Header("Skin:")]
    public BotWearSkinItems botWearSkinItems;
    [Header("Attack:")]
    public BotAttack botAttack;
    public bool isHaveWeapon;
    [Header("Indicator:")]
    public IndicatorTarget indicator;
    public BotNameUI botName;

    private IState currentState = new IdleState();

    private void Start()
    {
        centerOfMap = GameObject.FindGameObjectWithTag(Constant.CENTER_OF_MAP).transform;
        currentBodyMatType = (MaterialType)Random.Range(2, Colors.instance.mats.Length);
        SetSkinnedMeshRenderer(currentBodyMatType);
    }

    protected void Update()
    {
        currentState.OnExecute(this);
        if (isDead == true)
        {
            return;
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        ActiveNavmeshAgent();
        ActiveName();
        ActiveIndicator();
        SetSkinnedMeshRenderer(currentBodyMatType);
        ChangeState(new IdleState());
        botAttack.enemy = null;
        isDead = false;
    }

    public override void OnDeath()
    {
        DisableCollider();
        characterAnim.ChangeAnim(Constant.DIE);
        DeactiveName();
        DeactiveIndicator();
        UnDisplayOnHandWeapon();
        SetSkinnedMeshRenderer(MaterialType.Black);
        BotManager.instance.DespawnBotName(this);
        DeactiveIndicator();
        LevelManager.instance.DeleteCharacterInOtherEnemyLists(this);
        isDead = true;
    }

    public void StopMoving()
    {
        characterAnim.ChangeAnim(Constant.IDLE);
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped= true;
    }

    public void Move()
    {
        characterAnim.ChangeAnim(Constant.RUN);
        navMeshAgent.isStopped = false;
    }

    public void GoToCenterOfMap()
    {
        Cache.GetTransform(this.gameObject).LookAt(centerOfMap.position);
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public override void EnableCollider()
    {
        if(capsulCollider != null)
        capsulCollider.enabled = true;
    }

    public override void DisableCollider()
    {
        if(capsulCollider != null)
        capsulCollider.enabled = false;
    }

    public void ActiveNavmeshAgent()
    {
        if(navMeshAgent != null)
        navMeshAgent.enabled = true;
    }

    public void DeActiveNavmeshAgent()
    {
        if(navMeshAgent != null)
        navMeshAgent.enabled = false;
    }

    public void ActiveName()
    {
        if(botName != null)
        botName.gameObject.SetActive(true);
    }

    public void DeactiveName()
    {
        if(botName != null)
        botName.gameObject.SetActive(false);
    }

    public void ActiveIndicator()
    {
        if(indicator != null)
        indicator.enabled = true;
    }

    public void DeactiveIndicator()
    {
        if(indicator != null)
        indicator.enabled = false;
    }

}
