using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBowscript : MonoBehaviour
{
    private Rigidbody myBody;
    public float speed = 30f;
    public float deactivate_Timer = 3f;
    public float damage = 15f;

     void Awake()
    {
        myBody = GetComponent<Rigidbody>();    
    }

    void Start()

    {
        Invoke("DeactivateGameObject", deactivate_Timer);

    }
    public void Launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed;
        transform.LookAt(transform.position + myBody.velocity);
    }
void DeactivateGameObject()
        {
        if(gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
               
        }

    private void OnTriggerEnter(Collider target)
    {
        //after we touch the enemy deactivate the game object

    }


}// class
