using UnityEngine;

public class Hammer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var mole = other.GetComponentInParent<Mole>();
        
        if (!mole)
            return;
        
        mole.Hit();
    }
}
