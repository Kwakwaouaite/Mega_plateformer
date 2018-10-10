using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour {

	[SerializeField]
    private float rotationSpeed;

    private float yRot = 0;
	void Start () {
		
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
            Debug.Log("Game Ends !!");
    }

    // Update is called once per frame
    void Update () {
        yRot += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, yRot, 0);
	}
}
