using UnityEngine;
using System.Collections;
using System;

/*
 * このコンポーネントにくっつけたゲームオブジェクトに対してイージングを行う。
 */
public class ObjectMover : MonoBehaviour
{

	/*
	 * イージング関数一覧
	 * http://easings.net/ja
	 * 
	 * 作成参考: イージング関数式
	 * http://hkom.blog1.fc2.com/blog-entry-729.html
	 * 
	 * 作成参考: enum
	 * http://hiyotama.hatenablog.com/entry/2015/05/26/105629
	 * 
	 */
	public enum EasingFunction
	{
		Linear,
		EaseInQuad,
		EaseOutQuad,
		EaseInOutQuad
	}

	public GameObject target;//どのゲームオブジェクトに向かって？
	public float T0;//どれだけの時間(秒)で？
	public bool trigger;//trueにするとイージング発動
	public EasingFunction easingFunction;//イージング関数のタイプ
	
	
	float duration;//nowTime(s)
	Vector3 diff;//ターゲットとの距離ベクトル
	Vector3 initPos;//イージング前のこのゲームオブジェクトの初期位置
	bool inOutSwitch;//InOutを行うための真ん中切り替え(falseならIn、trueならOut中)
	float tempDuration;//InOutのOutからのduration

	// Use this for initialization
	void Start()
	{
		duration = 0;
		diff = target.transform.position - this.transform.position;
		initPos = this.transform.position;
		//Debug.Log(diff);
	}

	// Update is called once per frame
	void Update()
	{
		moveObject();//velocity(1flame)
	}


	void moveObject()
	{
		/*
		 *	easingする前 
		 */
		if (!trigger)
		{
			diff = target.transform.position - this.transform.position;
			initPos = this.transform.position;
			return;
		}

		/*
		 *	easingした後
		 */
		if (T0 < duration)
		{
			trigger = false;
			duration = 0;
			this.transform.position = target.transform.position;
			return;
		}


		switch (easingFunction)
		{
			case EasingFunction.Linear:
				this.transform.position =
					new Vector3(
						linear(duration, T0, diff.x),
						linear(duration, T0, diff.y),
						linear(duration, T0, diff.z)
					) + initPos;
				break;

			case EasingFunction.EaseInQuad:
				this.transform.position =
					new Vector3(
						easeInQuad(duration, T0, diff.x),
						easeInQuad(duration, T0, diff.y),
						easeInQuad(duration, T0, diff.z)
					) + initPos;
				break;

			case EasingFunction.EaseOutQuad:
				this.transform.position =
					new Vector3(
						easeOutQuad(duration, T0, diff.x),
						easeOutQuad(duration, T0, diff.y),
						easeOutQuad(duration, T0, diff.z)
					) + initPos;
				break;

			case EasingFunction.EaseInOutQuad:
				if (duration < T0 / 2)
				{
					inOutSwitch = false;
					this.transform.position =
					new Vector3(
						easeInQuad(duration, T0 / 2, diff.x / 2),
						easeInQuad(duration, T0 / 2, diff.y / 2),
						easeInQuad(duration, T0 / 2, diff.z / 2)
					) + initPos;
				}
				else
				{
					if (!inOutSwitch)
					{
						diff = target.transform.position - this.transform.position;
						initPos = this.transform.position;
						inOutSwitch = true;
						tempDuration = 0;
					}

					this.transform.position =
					new Vector3(
						easeOutQuad(tempDuration, T0 / 2, diff.x),
						easeOutQuad(tempDuration, T0 / 2, diff.y),
						easeOutQuad(tempDuration, T0 / 2, diff.z)
					) + initPos;

				}
				
				break;
		}

		
		/*
		 * 時間をすすめる
		 */	
		duration += Time.deltaTime;//nowTime(s)
		if (inOutSwitch)
		{
			tempDuration += Time.deltaTime; 
		}

	}

	


	/*
	 *	イージング関数一覧
	 *	function(時間t, 時間終了値T0, 変位終了値L0)
	 */
	float linear(float time, float T0, float L0)
	{
		float a = L0 / T0;
		return a * time;
	}


	float easeInQuad(float time, float T0, float L0)
	{
		float a = L0 / Mathf.Pow(T0, 2);
		return a * Mathf.Pow(time, 2);
	}

	float easeOutQuad(float time, float T0, float L0)
	{
		float a = L0 / Mathf.Pow(T0, 2);
		return -a * Mathf.Pow(time - T0, 2) + L0;
	}
	


}

