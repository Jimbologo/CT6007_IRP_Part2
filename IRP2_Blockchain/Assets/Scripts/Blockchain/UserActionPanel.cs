using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates player Action panel and manages action buttons
/// </summary>
public class UserActionPanel : MonoBehaviour
{
    public bool updateRequired = false;

    public NET_ClientList clientListData = null;

    [SerializeField]
    private Dropdown playerListDropdown = null;

    /// <summary>
    /// Main Update loop
    /// </summary>
    private void Update()
    {
        if(updateRequired)
        {
            UpdateDropdown();
        }
    }

    /// <summary>
    /// Updates the player selection dropdown with new ID's
    /// </summary>
    private void UpdateDropdown()
    {
        List<string> playerIDs = new List<string>();

        //We manually add the host ID
        playerIDs.Add("Player: " + -1);

        //add all known player ids to list
        for (int i = 0; i < clientListData.clients.Count; ++i)
        {
            playerIDs.Add("Player: " + clientListData.clients[i].ToString());
        }

        //add all known player ids to dropdown
        playerListDropdown.ClearOptions();
        playerListDropdown.AddOptions(playerIDs);

        Debug.Log("Client List Updated");

        updateRequired = false;
    }
}
