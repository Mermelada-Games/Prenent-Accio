using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private GameObject hook;
    private Fishing fishing;
    private Rigidbody2D hookedObjectRb;
    public List<GameObject> fishList = new List<GameObject>();
    public List<GameObject> trashList = new List<GameObject>();
    public bool isHooked = false;
    public int fishCount = 0;
    public int trashCount = 0;

    private void Start()
    {
        fishing = FindObjectOfType<Fishing>();
        fishing.UpdateText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isHooked = true;
        if (collision.gameObject.tag == "Fish") 
        {
            fishCount++;
            fishList.Add(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Trash") 
        {
            trashCount++;
            trashList.Add(collision.gameObject);
        }

        Rigidbody2D collidedRb = collision.GetComponent<Rigidbody2D>();
        Fish fish = collision.GetComponent<Fish>();
        if (collidedRb != null)
        {
            hookedObjectRb = collidedRb;

            hookedObjectRb.transform.parent = transform;

            if (collision.gameObject.tag == "Fish") fish.isHooked = true;
        }
        fishing.UpdateText();
        Debug.Log("TrashCount: " + trashCount + " FishCount: " + fishCount);
    }

}
