using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour {

	[SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private GameObject activateWhenEnd;

    private float yRot = 0;
	void Start () {
		
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Game Ends !!");
            activateWhenEnd.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update () {
        yRot += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, yRot);
	}
}
