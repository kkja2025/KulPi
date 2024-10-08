using UnityEngine;
 
public class LocationChange : Interactable
{
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        GameObject player = GameObject.FindWithTag("Player");
        if (collision.gameObject == player)
        {
            GameManager.Singleton.SetPlayerPosition(newPosition);
        }
    }
}