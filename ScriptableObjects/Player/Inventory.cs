﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : ScriptableObject {
    public ItemData.Type type;                                                  // Inventory type.
    
    [Header("Debug")]
    public bool restartAtInit;                                                  // Debug - clears the inventory when the game scene is loaded.

    [System.Serializable]
    public class InventoryItem {                                                // Inventory item class.
        public Item item;
        public int quantity;

        public InventoryItem( Item item, int quantity ) {
            this.item = item;
            this.quantity = quantity;
        }
    }

    [SerializeField]
    public List<InventoryItem> items = new List<InventoryItem>();                                           // List of items for this inventory.#

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="item">Item - crafting item to store in the inventory.</param>
    /// <param name="quantity">int - how many units will be stack in the inventory</param>
    /// <returns>bool</returns>
    public bool AddItem( Item item, int quantity = 1 ) {
        for ( int i = 0; i < items.Count; i++ ) {
            if ( items[i].item.data.id == item.data.id ) {
                // update quantity.
                items[i].quantity += quantity;

                // check if no more items can be stack.
                if ( items[i].quantity > items[i].item.data.maxStack ) {
                    items[i].quantity = items[i].item.data.maxStack;
                    return false;
                }
                return true;
            }
        }

        // if here, item is not in the inventory.
        bool allAdded = true;
        if ( quantity > item.data.maxStack ) {
            allAdded = false;
            quantity = item.data.maxStack;
        }

        InventoryItem newItem = new InventoryItem( item, quantity );
        items.Add( newItem );

        // add item gameObject reference to scene inventory.
        SceneInventory.instance.AddItem( item.gameObject.transform.parent.gameObject, item.data.type );

        return allAdded;
    }

    /// <sumamry>
    /// Update quantity.
    /// If the item stack is 0,
    /// the item will be removed from
    /// the inventary.
    /// </summary>
    /// <param name="itemID">int - item id.</param>
    /// <param name="type">ItemData.type - item type.</param>
    /// <param name="newQuantity">int - new item quantity. Default value to 1</param>
    /// <returns>string</returns>
    public string UpdateQuantity( int itemID, int quantity = 1 ) {
        string response = "Not found";

        for ( int i = 0; i < items.Count; i++ ) {
            if ( itemID == items[i].item.data.id ) {
                    
                // check if full.
                if ( items[i].quantity == items[i].item.data.maxStack ) {
                    response = "full";
                    return response;
                }

                items[i].quantity += quantity;

                // check if over stack.
                if ( items[i].quantity > items[i].item.data.maxStack ) {
                    items[i].quantity = items[i].item.data.maxStack;
                    response = "added";
                    return response;
                }

                // check if no more stack, then remove item.
                if ( items[i].quantity <= 0 ) {
                    bool removed = RemoveItem( itemID );
                    response = ( removed ) ? "removed" : "Not found";
                    return response;
                }
                    
            }
        }

        return response;
    }
    

    /// <summary>
    /// Remove item from 
    /// the player's inventary.
    /// </summary>
    /// <param name="itemID">int - item id</param>
    /// <param name="type">ItemData.Type - item type.</param>
    /// <returns>bool</returns>
    public bool RemoveItem( int itemID ) {
        Item itemToRemove = null;
        int index = -1;
        
        for ( int i = 0; i < items.Count; i++ ) {
            if ( items[i].item.data.id == itemID ) {
                index = i;
                itemToRemove = items[i].item;
            }
        }

        if ( index > -1 && itemToRemove != null ) {
            // remove item from physical inventory in the scene.
            SceneInventory.instance.RemoveItem( itemToRemove.gameObject.transform.parent.gameObject, itemToRemove.data.type );

            // remove from inventory list.
            items.RemoveAt( index );
            return true;
        }

        return false;
    }

    /// <summary>
    /// Get item.
    /// </sumamry>
    /// <param name="itemId">int - itemID.</param>
    /// <returns>Item</returns>
    public Item GetItem( int itemId ) {
        Item item = null;
        
        for ( int i = 0; i < items.Count; i++ ) {
            if ( items[i].item.data.id == itemId ) {
                item = items[i].item;
                break;
            }
        }

        return item;
    }

    /// <summary>
    /// Get item quantity
    /// </sumamry>
    /// <param name="itemId">int - itemID.</param>
    /// <returns>int</returns>
    public int GetItemCurrentQuantity( int itemId ) {
        int quantity = 0;
        
        for ( int i = 0; i < items.Count; i++ ) {
            if ( items[i].item.data.id == itemId ) {
                quantity = items[i].quantity;
                break;
            }
        }

        return quantity;
    }

    /// <summary>
    /// Reset iventory.
    /// </summary>
    private void Reset() {
        items.Clear();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init() {
        if ( items == null ) {
            items = new List<InventoryItem>();
        }

        if ( restartAtInit ) {
            Reset();
        }
    }
}
