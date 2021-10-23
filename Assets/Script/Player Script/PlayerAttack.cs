using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weapon_Manager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;

    [SerializeField]
    private Transform arrow_Bow_StartPosition;

    void Awake()
    {
        weapon_Manager = GetComponent<WeaponManager>();


        //zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;


    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        WeaponShoot();
        ZoomInAndOut();
    } //

    void WeaponShoot()
    {
        if (weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if (Input.GetMouseButton(0))          //               && Time.time > nextTimeToFire 
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFired();
            }
            
        }

        // if we have a regual weapon that shoot once
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                //handle axe
                if (weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }
                // handle shoot
                else if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                else
                {
                    // we have an arrow or spear
                    if (is_Aiming)
                    {
                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                        if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowOrSpear(true); // throw arrow
                        }
                        else if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowOrSpear(false); //throw sprear
                        }
                    }


                }// else
            } // if input get mouse button 0  

        }// else



    } // weapon shoot

    void ZoomInAndOut()
    {
        // we are going to aim with our camera on the weapon
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            // if we press and hold right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTag.ZOOM_IN_ANIM);

                crosshair.SetActive(false);
            }
            //
            if (Input.GetMouseButtonUp(1))
            {
                zoomCameraAnim.Play(AnimationTag.ZOOM_OUT_ANIM);
                crosshair.SetActive(true);
            }
            if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
                    is_Aiming = true;
                }
                if (Input.GetMouseButtonUp(1))
                {
                    weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                    is_Aiming = false;
                }
            }

        }
    } // zoom in and out       

    void ThrowArrowOrSpear(bool throwArrow)
    {
        if (throwArrow)
        {
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrow_Bow_StartPosition.position;
            arrow.GetComponent<ArrowBowscript>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_Bow_StartPosition.position;
            spear.GetComponent<ArrowBowscript>().Launch(mainCam);
        }
    }    // throw arrow and spear

    void BulletFired()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            if(hit.transform.tag==Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }

    }// bullet fired



} // class
