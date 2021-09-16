﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gasket : MonoBehaviour
{
    public bool frontPanelgasket;
    public List<Follow> linePoints = new List<Follow>();
    public GameObject staticGasket;
    public GameObject guide;

    private void Start()
    {
        //get all besides the first point
        foreach (Transform trans in this.GetComponentInChildren<Transform>())
               if(trans.name != "LinePoint")
                    linePoints.Add(trans.GetComponent<Follow>());
    }

    public void GasketHooked()
    {
        guide.SetActive(false);
        if(!frontPanelgasket)
            staticGasket.SetActive(false);
        this.GetComponent<LineRenderer>().enabled = true;
        StartCoroutine(AdjustFollowSmooth());
    }

    IEnumerator AdjustFollowSmooth()
    {
        float offset = 0;
        yield return new WaitForSeconds(0.65f);
        foreach (Follow linePoint in linePoints)
            linePoint.smoothPos = 0.015f;
        yield return new WaitForSeconds(0.25f);
        foreach (Follow linePoint in linePoints)
        {
            if (frontPanelgasket)
            {
                offset = Random.Range(-0.08f, 0.08f);
                linePoint.offset = new Vector3(offset, 0.08f, offset);
            }
            else
            {
                offset -= 0.0025f;
                linePoint.offset = new Vector3(0, offset, 0);
            }
        }
    }

    public void DropGasket()
    {
        Destroy(this.gameObject);
        //foreach(Follow point in linePoints)
        //{
        //    point.enabled = false;
        //    point.gameObject.GetComponent<Follow>().enabled = false;
        //    point.gameObject.GetComponent<Collider>().enabled = true;
        //    point.gameObject.GetComponent<Collider>().isTrigger = false;
        //    point.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //}
    }
}
