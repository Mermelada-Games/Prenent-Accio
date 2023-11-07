using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private GameObject hook;
    private Rigidbody2D hookedObjectRb;
    public bool isHooked = false;
    public int fishCount = 0;
    public int trashCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isHooked = true;
        if (collision.gameObject.tag == "Fish") fishCount++;
        else if (collision.gameObject.tag == "Trash") trashCount++;

        Rigidbody2D collidedRb = collision.GetComponent<Rigidbody2D>();
        if (collidedRb != null)
        {
            hookedObjectRb = collidedRb;

            hookedObjectRb.transform.parent = transform;
        }
        Debug.Log("TrashCount: " + trashCount + " FishCount: " + fishCount);
    }

}