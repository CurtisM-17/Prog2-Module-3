using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : Villager
{
    public GameObject knifePrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    Coroutine dashing;

    public float dashSpeed = 7f;

    bool isDashing = false;

	protected override void Attack()
    {
        //if (isDashing) return;

        //dash towards mouse
        destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (speed != dashSpeed) defaultSpeed = speed;

        if (dashing != null) StopCoroutine(dashing);

        //speed = dashSpeed;
        dashing = StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        //isDashing = true;

        speed = dashSpeed;

        while (speed > defaultSpeed)
        {
            yield return null;
        }

		base.Attack();
		yield return new WaitForSeconds(0.1f);
		Instantiate(knifePrefab, spawnPoint1.position, spawnPoint1.rotation);
		yield return new WaitForSeconds(0.2f);
		Instantiate(knifePrefab, spawnPoint2.position, spawnPoint2.rotation);

		speed = defaultSpeed;

		yield return new WaitForSeconds(0.5f);
		//isDashing = false;

    }

    public override ChestType CanOpen()
    {
        return ChestType.Thief;
    }
}
