using UnityEngine;

public class PlayerKeyItem : MonoBehaviour, IItem
{
    public void Collect()
    {
        Destroy(gameObject);
    }
}
