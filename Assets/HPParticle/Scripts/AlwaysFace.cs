/*
this script is attached to ammo, and stores data to be used by the PickUpItemScript
*/

using UnityEngine;
using System.Collections;

public class AlwaysFace : MonoBehaviour {


	[HideInInspector]public GameObject target;
	public float Speed;

	public bool JustOnStart = false;

	// turn towards target
	void Start() 
	{
		StartCoroutine(LateStart());
	}

	IEnumerator LateStart() {
		yield return new WaitForSeconds(1f);
		target = GameObject.FindWithTag("MainCamera");
		if (target != null) 
		{
            Vector3 dir = target.transform.position - transform.position;
            Quaternion Rotation = Quaternion.LookRotation(dir);
            gameObject.transform.rotation = Rotation;
        }
    }
	
	// turn towards target
	void FixedUpdate () 
	{
		if (target == null)
		{
			return;
		}

		if (JustOnStart == false)
		{
			Vector3 dir = target.transform.position - transform.position;
			Quaternion Rotation = Quaternion.LookRotation(dir);

			gameObject.transform.rotation = Quaternion.Lerp (gameObject.transform.rotation,Rotation,Speed * Time.deltaTime);
		}
	}
}
