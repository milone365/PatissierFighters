using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	
	public bool CollidedWithSlime;
    JellyCube jellycube;
	public enum SlimeSide {None, Left, Right}
	public SlimeSide curSlimeSide;
    Controller controller;
	float slimeSideForce = 10000f;
	float slimeUpForce = 6000f;
	float strafeForce = 2500f;
	float jumpForce = 500f;

	Vector3 Dir;

	Rigidbody rigid;

	GameObject Slime;

	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
        jellycube = GetComponentInParent<JellyCube>();
        controller = GetComponent<Controller>();
	}
	
	void FixedUpdate()
	{
		SlimeLocomotion();

		Dir = Dir * Time.deltaTime;

		rigid.AddForce(Dir, ForceMode.Force);

		Dir = Vector3.zero;

		/*if(TouchController.Instance.curSwipeDir != TouchController.SwipeDir.None)
		{
			//foreach(Controller contr in controller)
				//Locomotion();
		}*/
	}

	void SlimeLocomotion()
	{
		if(curSlimeSide == SlimeSide.Left)
		{
			Dir += Vector3.left * slimeSideForce;
		}
		if(curSlimeSide == SlimeSide.Right)
		{
			Dir += Vector3.right * slimeSideForce;
		}
		if(curSlimeSide == SlimeSide.None)
		{
			Dir = Vector3.zero;
		}
		
		if(CollidedWithSlime == true)
		{
			Dir += Vector3.up * slimeUpForce;
		}

		controller.Dir = Dir;
	}

	public void Locomotion()
	{
		Vector3 Locomotion = Vector3.zero;

		if(TouchController.Instance.curSwipeDir == TouchController.SwipeDir.Right)
		{
			Locomotion = Vector3.right * strafeForce;
		}
		if(TouchController.Instance.curSwipeDir == TouchController.SwipeDir.Left)
		{
			Locomotion = Vector3.left * strafeForce;
		}

		Locomotion = Locomotion * Time.deltaTime;

		rigid.AddForce(Locomotion, ForceMode.Force);
	}

	public void Jump()
	{
		Vector3 JumpV = new Vector3(0,jumpForce,0);
		
		JumpV = JumpV * Time.deltaTime;
		
		rigid.AddForce(JumpV, ForceMode.Impulse);
	}



	void OnCollisionStay (Collision col)
	{
		if(col.gameObject.tag == "Slime") 
		{
			Slime = col.gameObject;

			if(Slime.transform.position.x > jellycube.Center.x)
			{
				curSlimeSide = SlimeSide.Right;
			}
			else if(Slime.transform.position.x < jellycube.Center.x)
			{
				curSlimeSide = SlimeSide.Left;
			}

			CollidedWithSlime = true;
		}
	}
	void OnCollisionExit (Collision col)
	{
		if(col.gameObject.tag == "Slime")
		{
			CollidedWithSlime = false;
			curSlimeSide = SlimeSide.None;
		}
	}
}
