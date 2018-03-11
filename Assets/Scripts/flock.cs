using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : MonoBehaviour {

    public float speed = 0.001f;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averageSpeed;
    float neighborDistance = 2.0f;

    public float speedMult = 1;

	// Use this for initialization
	void Start () {
        speed = Random.Range(0.5f, 1);
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, Vector3.zero) >= globalFlock.tankSize)
        {
            Vector3 direction = Vector3.zero - this.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1) * speedMult;
        }
        else
        {
            if (Random.Range(0, 5) < 1)
            {
                Flocking();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed * speedMult);
	}

    void Flocking()
    {
        GameObject[] gos = globalFlock.allAnimals;

        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = globalFlock.goalPos;

        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighborDistance)
                {
                    vcenter += go.transform.position;
                    groupSize++;
                    if (dist < 1.0f)
                    {
                        vavoid += this.transform.position - go.transform.position;
                    }

                    flock anotherFlock = go.GetComponent<flock>();
                    gSpeed += anotherFlock.speed;
                }
            }

            if (groupSize > 0)
            {
                vcenter = vcenter / groupSize + (goalPos - this.transform.position);
                speed = gSpeed / groupSize * speedMult;

                Vector3 direction = vcenter + vavoid - transform.position;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                                                          Quaternion.LookRotation(direction),
                                                          rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
