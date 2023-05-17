using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Joystick:")]
    [SerializeField] private Joystick joystick;

    [Header("Animation:")]
    [SerializeField] private CharacterAnimation characterAnimation;

    [Header("Mouse:")]
    private Vector3 firstMousePosition;
    private Vector3 currentMousePosition;
    private Vector3 direction;

    [Header("Player Properties:")]
    [SerializeField] private Player player;
    [SerializeField] private float speed;
    public Rigidbody rb;

    public void OnInit()
    {
        joystick = FindObjectOfType<Joystick>();
    }

    void Update()
    {
        if (LevelManager.instance.isGaming == false)
        {
            return;
        }

        if(!joystick.gameObject.activeInHierarchy)
        {
            return;
        }

        if (player.isDead == true)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            firstMousePosition = Input.mousePosition;
            Cache.GetTransform(joystick.gameObject).position = firstMousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            currentMousePosition = Input.mousePosition;
            direction = currentMousePosition - firstMousePosition;
            Cache.GetTransform(joystick.joystickHandle.gameObject).position = currentMousePosition;
            if (Vector3.Distance(Cache.GetTransform(joystick.joystickHandle.gameObject).position, Cache.GetTransform(joystick.joystickBackground.gameObject).position) > joystick.joystickRadius)
            {
                Cache.GetTransform(joystick.joystickHandle.gameObject).position = Cache.GetTransform(joystick.joystickBackground.gameObject).position - (Cache.GetTransform(joystick.joystickBackground.gameObject).position - Cache.GetTransform(joystick.joystickHandle.gameObject).position).normalized * joystick.joystickRadius;
            }
            if (Vector3.Distance(Cache.GetTransform(joystick.joystickHandle.gameObject).position, Cache.GetTransform(joystick.joystickBackground.gameObject).position) > joystick.joystickRadius / 2)
            {
                Cache.GetTransform(this.gameObject).rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
                rb.velocity = new Vector3(direction.x, 0, direction.y).normalized * speed;
                characterAnimation.ChangeAnim(Constant.RUN);
            }
            player.isMoving = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            characterAnimation.ChangeAnim(Constant.IDLE);
            Cache.GetTransform(joystick.gameObject).position += new Vector3(10000, 0, 0);//hide joystick
            rb.velocity = new Vector3(0, 0, 0);
            player.isMoving = false;
        }
        
    }
}
