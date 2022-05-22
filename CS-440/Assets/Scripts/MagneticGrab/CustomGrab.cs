using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomGrab : MonoBehaviour
{
    //Ray Cast Origin
    public GameObject RayCastPosition;

    //The other hand
    public GameObject OtherHandObj;
    private CustomGrab OtherHand;

    //Controller
    public OVRInput.Controller Controller;
    //Snap Positions
    public Transform[] SnapPosition;
    
    //private bool MaterialChanged = false;
    //private bool ColliderExited = false;
    private bool ItemInHand = false;

    //Item to Interact with
    private GameObject ItemInFocus;
    public GameObject GrabbedItem;
    private Rigidbody GrabbedItemRigidBody;

    //LineRenderer
    public LineRenderer LineRenderer;
    private float LineWidth = 0.02f;
    private float lineMaxLength = 100f;
    private Vector3[] InitLaserPositions;


    void Start()
    {
        //Initialize variables
        InitLaserPositions = new Vector3[2] {Vector3.zero, Vector3.zero};
        LineRenderer = GetComponent<LineRenderer>();
        OtherHand = OtherHandObj.GetComponent<CustomGrab>();
    }

    // Update is called once per frame
    void Update()
    {
        RenderLine();
        GrabItem();
        DropItem();
    }

    private void DropItem(){
        if(OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, Controller) && ItemInHand == true) {
            ItemInHand = false;
            GrabbedItem.transform.parent = null;
            GrabbedItemRigidBody = GrabbedItem.GetComponent<Rigidbody>();
            GrabbedItemRigidBody.useGravity = true;;
            GrabbedItemRigidBody.isKinematic = false;
            GrabbedItem = null;
        }
    }

    private void GrabItem() {
        //Grab item pointed at item if it is not already grabbed
        if(ItemInHand == false && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, Controller) && ItemInFocus != null && OtherHand.GrabbedItem != ItemInFocus) {
            GrabbedItem = ItemInFocus;
            ItemInHand = true;
            //Get snapp position and place
            //If the object has a tag correposing to one of the stored snap Positions, then
            //we grab the item and set it to the given snapp position
            var snapp = SnapPosition.Where(x => x.CompareTag(GrabbedItem.tag)).FirstOrDefault();
            if(snapp != null) {
                GrabbedItem.transform.parent = snapp.transform;
                GrabbedItem.transform.position = snapp.position;
                GrabbedItem.transform.rotation = snapp.rotation;
                GrabbedItemRigidBody = GrabbedItem.GetComponent<Rigidbody>();
                GrabbedItemRigidBody.useGravity = false;
                GrabbedItemRigidBody.isKinematic = true;
                LineRenderer.enabled = false;
            }
        }
    }

    private void RenderLine() {
        if(GrabbedItem == null) {
            int layerMask = 1 << 11;

            RaycastHit hit;
            //Does ray intersect with any object in the "Grabbable" layer (layer 11)
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2, layerMask)){
                LineRenderer.enabled = true;
                LineRenderer.SetPositions(InitLaserPositions);
                LineRenderer.startWidth = LineWidth;
                LineRenderer.endWidth = LineWidth;
                RenderLine(RayCastPosition.transform.position, transform.TransformDirection(Vector3.forward), lineMaxLength);
                if(ItemInFocus == null) {
                    ItemInFocus = hit.collider.gameObject;
                }
            }
            else {
                LineRenderer.enabled = false;
                ItemInFocus = null;
            }
        }
    }

    void RenderLine(Vector3 targetPosition, Vector3 direction, float length){
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length*direction);

        if(Physics.Raycast(ray, out raycastHit, length)){
            endPosition = raycastHit.point;
        }

        LineRenderer.SetPosition(0, targetPosition);
        LineRenderer.SetPosition(1, endPosition);
    }
}