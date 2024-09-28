using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herb : Interactable

{

public override void Interact()

 {

        Debug.Log("Collected herb!");
        // For example, you could add the herb to the player's inventory
        CollectHerb();
        // Log before destroying the herb to verify if the method is called

        Debug.Log("Destroying herb: " + gameObject.name);
        GameManager.Singleton.RemoveObject(gameObject);
 }

    // Example method to simulate adding herb to inventory

 private void CollectHerb()

    {

        Debug.Log("Herb added to inventory");

        // Add your inventory logic here
        InventoryManager.Singleton.AddItem("Bush 1 (no flowers) - GREEN", gameObject.name); 


    }

}
