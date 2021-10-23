﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Net.Sockets;
using UnityEditor.VersionControl;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}


public class EnemyController : MonoBehaviour
{
    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;

    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    public float chase_Distance = 7f;
    private float currunt_Chase_Distance = 2f;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;

    private EnemyAudio enemy_Audio;

     void Awake()
    {
        enemy_Anim = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        enemy_Audio = GetComponent<EnemyAudio>();
    }


    void Start()
    {

        enemy_State = EnemyState.PATROL;

        patrol_Timer = patrol_For_This_Time;
        //when the enemy first gets to the player
        //attack right away
        attack_Timer = wait_Before_Attack;
        //memorize the value of chase dis
        //so that we can put it back
        currunt_Chase_Distance = chase_Distance;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }
        if(enemy_State == EnemyState.CHASE)
        {
            Chase();
        }
        if(enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }
    }

    void Patrol()
    {
        // tell nav agent that he can move
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        //add to the patrol timer
        patrol_Timer += Time.deltaTime;
        if(patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }

        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else
        {
            enemy_Anim.Walk(false);
        }
        // distance btw the [;ayer and the enemy

        if(Vector3.Distance(transform.position,target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false); 
            enemy_State = EnemyState.CHASE;
            // play spoted audio

            enemy_Audio.Play_ScreamSound();
        }
    }
    void Chase()
    {
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;

        // set the player's postion as the destination 
        // bcz we are chasing (running towards) the palyer
        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Run(true);
        }
        else
        {
            enemy_Anim.Run(false);
        }
        //if the dis btw enemy and the player is less than attack distance
        if(Vector3.Distance(transform.position,target.position) <= attack_Distance)
        {
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            // rest the chase distance to previous
            if(chase_Distance != currunt_Chase_Distance)
            {
                chase_Distance = currunt_Chase_Distance;
            }
        }
        else if(Vector3.Distance(transform.position, target.position) > chase_Distance)
        { 
            //palyer run away from enemy
            
            //stop running 
            enemy_Anim.Run(false);

            enemy_State = EnemyState.PATROL;

            // rest the patrol timer so that the function
            // can calculate the new patrol destination right away
            patrol_Timer = patrol_For_This_Time;

            //reset the chase dis
            if(chase_Distance != currunt_Chase_Distance)
            {
                chase_Distance = currunt_Chase_Distance;
            }



        }// else

        

    }//chase
    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;

        if(attack_Timer > wait_Before_Attack)
        {
            enemy_Anim.Attack();
            attack_Timer = 0f;
            //play attack sound

            enemy_Audio.Play_AttackSound();
        }

        if(Vector3.Distance (transform.position, target.position) > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }


    }// attack

    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);
        Vector3 randDir = Random.insideUnitSphere * rand_Radius;

        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        navAgent.SetDestination(navHit.position);

    }

    void Turn_On_AttackPoint()
    {
        attack_Point.SetActive(true);
    }
    void Turn_Oof_AttackPoint()
    {
        if (attack_Point.activeInHierarchy)
        {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State
    {
        get; set;


    }

}// class