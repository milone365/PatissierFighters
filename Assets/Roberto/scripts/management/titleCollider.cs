using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleCollider : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IntroCharacter>())
        {
            IntroCharacter character = other.GetComponent<IntroCharacter>();
            character.Play();
        }
    }
}
