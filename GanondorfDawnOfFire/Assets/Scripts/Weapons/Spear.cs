using UnityEngine;
using System.Collections;

public class Spear : Weapon
{
    public override IEnumerator Animation()
    {
        running = true;
        while(runForward)
        {
            transform.position += transform.forward * Time.deltaTime;
            timer += Time.deltaTime;
            if (timer >= 3f)
                runForward = false;
            yield return 0;
        }
        
		while(!runForward)
        {
            transform.position -= transform.forward * Time.deltaTime;
            timer -= Time.deltaTime;
            if (timer <= 0f)
                runForward = true;
            yield return 0;
        }

		
    }
}
