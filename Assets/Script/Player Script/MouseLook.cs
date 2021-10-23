using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{   [SerializeField]
    private Transform playerRoot, lookRoot;
    [SerializeField]
    private bool invert;
    [SerializeField]
    private bool can_Unlock = true;
    [SerializeField]
    private float sensitivity = 5f;
    [SerializeField]
    private int smooth_Steps = 10;
    [SerializeField]
    private float smooth_Weight = 0.4f;
    [SerializeField]
    private float roll_Angle = 10f;
    [SerializeField]
    private float roll_Speed = 3f;
    [SerializeField]
    private Vector2 default_Look_Limits = new Vector2(-70f, 80f);

    private Vector2 look_Angle;
    private Vector2 current_Mouse_Look;
    private Vector2 smooth_Move;

    private float current_Roll_Angel;
    private int last_look_Frame;
      
       void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        LoclAndUnlockCursor();

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    void LoclAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.AltGr))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                    
            }
        } 
            
    }// lock and unlock

    void LookAround()
    {
        current_Mouse_Look = new Vector2(Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));
        look_Angle.x += current_Mouse_Look.x * sensitivity * (invert ? 1f : -1f);
        look_Angle.y += current_Mouse_Look.y * sensitivity;

        look_Angle.x = Mathf.Clamp(look_Angle.x, default_Look_Limits.x, default_Look_Limits.y);
        current_Roll_Angel =  Mathf.Lerp(current_Roll_Angel, Input.GetAxisRaw(MouseAxis.MOUSE_X) * roll_Angle, Time.deltaTime * roll_Speed);

        lookRoot.localRotation = Quaternion.Euler(look_Angle.x, 0f, current_Roll_Angel);
        playerRoot.localRotation = Quaternion.Euler(0f, look_Angle.y, 0f);
    
    
    }




}// class
