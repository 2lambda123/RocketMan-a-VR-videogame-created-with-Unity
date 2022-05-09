using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum eColor
{
    Red,
    Yellow,
    Green,
    Blue,
}

public class ButtonTrap : MonoBehaviour
{
    [SerializeField]
    public Vector3 pressedPos;
    public Vector3 unpressedPos;
    public GameObject Door;
    public GameObject button;
    public ButtonLamp buttonLamp;
    GameObject presser;
    AudioSource sound;
    public bool isPressed;
    public bool enabled;
    public ButtonLamp lamp;
    public TrapDoorController trapDoorController;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
        enabled = false;

        lamp = button.GetComponent<ButtonLamp>();
    }

    private void OnTriggerEnter(Collider other) {
        if(!isPressed && other.gameObject.tag == "buttonPresser") {
            
            //button.transform.position = pressedPos;
            StartCoroutine(MoveOverSpeed(button, pressedPos, 0.1f, true));
            presser = other.gameObject;
            if (enabled )
            {
                trapDoorController.setIsOpen(true);
                GameManager.Instance.UpdateGameState(RiddlesProgress.TrapDoorButton);
            }
            
            sound.Play();
            
        }
    }

    private void OnTriggerExit(Collider other) {
        if(isPressed && other.gameObject == presser) {
            
            //button.transform.position = unpressedPos;
            StartCoroutine(MoveOverSpeed(button, unpressedPos, 0.1f, false));
            if (enabled)
            {
                trapDoorController.setIsOpen(false);
            }
            
        }
    }

    public void enableButton()
    {
        enabled = true;
        lamp.on = true;
        lamp.lightColor = ButtonLamp.eColor.Green;
    }

    
    public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed, bool isPressedd)
    {
        /*
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        
           
        }

        */
        
        button.transform.position = end;
        yield return new WaitForSeconds(0.5f);
        isPressed = isPressedd;

    }
    
}

