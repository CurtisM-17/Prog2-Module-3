using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
	Vector3 scaleGoal = new(1, 1, 1);
	public float scaleSpeed = 0.1f;
	public float scaleIncrement = 0.1f;

	float scaleProgress = 0;

	private void Start()
	{
		foreach (Transform child in transform)
		{
			child.localScale = Vector3.zero; // Make tiny

		}

		Debug.Log("Animation will start in two seconds"); // Because Unity lags on startup

		StartCoroutine(ScaleObjects());
	}

	IEnumerator ScaleObjects()
	{
		yield return new WaitForSeconds(2);

		foreach (Transform child in transform)
		{
			while (scaleProgress < 1)
			{
				scaleProgress += scaleIncrement;

				child.localScale = Vector3.Lerp(Vector3.zero, scaleGoal, scaleProgress);

				yield return new WaitForSeconds(scaleSpeed);
			}

			child.localScale = scaleGoal; // Make sure it matches
			scaleProgress = 0;
		}
	}
}
