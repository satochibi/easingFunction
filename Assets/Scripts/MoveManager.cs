using UnityEngine;
using System.Collections;

public class MoveManager : MonoBehaviour {

	ObjectMover[] movers;
	int index;

	// Use this for initialization
	void Start () {
		movers = this.GetComponents<ObjectMover>();
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {

		bool totalTrigger = false;

		foreach (ObjectMover mover in movers)
		{
			totalTrigger |= mover.trigger;
		}

		if (!totalTrigger)
		{

			if (movers.Length <= index)
			{
				index = 0;
			}

			movers[index].trigger = true;
			index++;

			
		}
		
	}
}
