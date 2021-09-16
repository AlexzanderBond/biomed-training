﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapInPlace : MonoBehaviour
{
    public string objectType;
    public bool snapped;
    public UnityEvent OnSnapped;

    OVRGrabbable grabbable;
    Collider grabbableCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (!snapped)
        {
            if (other.GetComponent<OVRGrabbable>() || other.transform.parent.GetComponent<OVRGrabbable>()) 
            {
                if (other.GetComponent<OVRGrabbable>())
                {
                    grabbable = other.GetComponent<OVRGrabbable>();
                    grabbableCollider = other;
                }
                else if (other.transform.parent.GetComponent<OVRGrabbable>())
                {
                    grabbable = other.transform.parent.GetComponent<OVRGrabbable>();
                    grabbableCollider = other;
                }

                if (grabbable.objectType == objectType)
                {
                    //force release the object
                    if(grabbable.grabbingHand)
                        grabbable.grabbingHand.GetComponent<OVRGrabber>().ForceRelease(grabbable);

                    //disable collider and physics
                    other.enabled = false;
                    grabbable.GetComponent<Rigidbody>().isKinematic = true;
                    grabbable.GetComponent<Outline>().enabled = false;
                    grabbable.isTouchable = false;
                    grabbable.isGrabbable = false;

                    //snap to position
                    grabbable.transform.position = this.transform.position;
                    grabbable.transform.rotation = this.transform.rotation;

                    //audio
                    this.GetComponent<AudioSource>().Play();

                    StartCoroutine(DisableObject());

                    OnSnapped.Invoke();

                    snapped = true;
                }
            }
        }
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(0.1f);
        if (this.GetComponent<Renderer>())
            this.GetComponent<Renderer>().enabled = false;
        if (this.transform.GetChild(0))
            if (this.transform.GetChild(0).GetComponent<Renderer>())
                this.transform.GetChild(0).GetComponent<Renderer>().enabled = false;

        this.GetComponent<Collider>().enabled = false;

        //this.gameObject.SetActive(false);
    }
}
