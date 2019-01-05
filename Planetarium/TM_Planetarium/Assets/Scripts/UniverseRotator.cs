using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseRotator : MonoBehaviour
{
	public enum Player {
		A,
		B
	}

	public float rotateSpeed = 0.01f;

	public Player whichPlayer = Player.A;

	private KeyCode upKey;
	private KeyCode downKey;
	private KeyCode leftKey;
	private KeyCode rightKey;
	private KeyCode actionAKey;
	private KeyCode actionBKey;

    public Transform LookTarget;

    public List<ConstellationLine> constellations;


	// Use this for initialization
	void Start () {
		if (whichPlayer == Player.A){
			upKey = KeyCode.W;
			downKey = KeyCode.S;
			leftKey = KeyCode.A;
			rightKey = KeyCode.D;
			actionAKey = KeyCode.Q;
			actionBKey = KeyCode.E;
		} else {
			upKey = KeyCode.I;
			downKey = KeyCode.K;
			leftKey = KeyCode.J;
			rightKey = KeyCode.L;
			actionAKey = KeyCode.U;
			actionBKey = KeyCode.O;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey(upKey)){
            LookTarget.RotateAround(Vector3.zero, Vector3.right, rotateSpeed * Time.deltaTime);
			
		}

		if (Input.GetKey(downKey)){
			LookTarget.RotateAround(Vector3.zero, Vector3.right, -rotateSpeed * Time.deltaTime);
		}

		if (Input.GetKey(leftKey)){
			LookTarget.RotateAround(Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
		}

		if (Input.GetKey(rightKey)){
			LookTarget.RotateAround(Vector3.zero, Vector3.up, -rotateSpeed * Time.deltaTime);
		}

        transform.LookAt(LookTarget, Vector3.up);

		if (Input.GetKeyDown(actionAKey)){
			constellations.ForEach((ConstellationLine l) =>{
                l.Flash();
            });
		}

		if (Input.GetKeyDown(actionBKey)){
			// do something else!
		}
	}
}
