using UnityEngine;

public class PlayerKeyItem : MonoBehaviour, IItem
{
    public void Collect()
    {
        SFXManager.Play("Collect");

        Destroy(gameObject);
    }
}
