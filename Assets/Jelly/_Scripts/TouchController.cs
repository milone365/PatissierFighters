using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	public static TouchController Instance;

	public float minSwipeDistY;
	public float minSwipeDistX;

	float StrafeArea = 0f;

	public enum SwipeDir {None, Left, Right}
	public SwipeDir curSwipeDir;

	bool rTouchArea = false;
	bool lTouchArea = false;

	public Controller[] controller;
	
	private Vector2 startPos;

	void Awake()
	{
		Instance = this;

		controller = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Controller>();

		StrafeArea = Screen.width/3;
	}

	void FixedUpdate()
	{
		if(rTouchArea == true)
		{
			curSwipeDir = SwipeDir.Right;
		}
		if(lTouchArea == true)
		{
			curSwipeDir = SwipeDir.Left;
		}
		if(rTouchArea == false && lTouchArea == false)
		{
			curSwipeDir = SwipeDir.None;
		}
		//#if UNITY_ANDROID
		if(Input.touchCount > 0) 
		{
			foreach(Touch iTouch in Input.touches)
			{
				Touch touch = iTouch;

				switch (touch.phase) 
					
				{
				case TouchPhase.Began:
					startPos = touch.position;

					Debug.Log(startPos);

					if(startPos.x > StrafeArea && startPos.x < Screen.width - StrafeArea)
					{
						foreach(Controller contr in controller)
							contr.Jump();
					}

					if(startPos.x < StrafeArea)
					{
						lTouchArea = true;
					}

					if(startPos.x > Screen.width - StrafeArea)
					{
						rTouchArea = true;
					}
					break;

				case TouchPhase.Ended:
					
//					float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
//
//					float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
//
//					if (swipeDistVertical > minSwipeDistY && swipeDistVertical > swipeDistHorizontal
//					    && startPos.x > StrafeArea && startPos.x < Screen.width - StrafeArea) 
//					{
//						float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
//						
//						if (swipeValue > 0)//up swipe
//						{
//							foreach(Controller contr in controller)
//								contr.Jump();
//						}
//							
//						else if (swipeValue < 0)//down swipe
//							return;
//					}

					if(touch.position.x < StrafeArea)
					{
						lTouchArea = false;
					}
					
					if(touch.position.x > Screen.width - StrafeArea)
					{
						rTouchArea = false;
					}

	//				if (swipeDistHorizontal > minSwipeDistX && swipeDistHorizontal > swipeDistVertical) 
	//					
	//				{
	//					
	//					float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
	//					
	//					if (swipeValue > 0)//right swipe
	//					{
	//						curSwipeDir = SwipeDir.Right;
	//
	//						foreach(Controller contr in controller)
	//							contr.Locomotion();
	//					}
	//
	//					else if (swipeValue < 0)//left swipe
	//					{
	//						curSwipeDir = SwipeDir.Left;
	//
	//						foreach(Controller contr in controller)
	//							contr.Locomotion();
	//					}
	//							
	//				}
					break;
				}
			}
		}
	}
}
