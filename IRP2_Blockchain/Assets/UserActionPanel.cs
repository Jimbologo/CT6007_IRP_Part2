using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserActionPanel : MonoBehaviour
{
    public bool updateRequired = false;

    public NET_ClientList clientListData;

    [SerializeField]
    private Dropdown playerListDropdown;

    private void Update()
    {
        if(updateRequired)
        {
            UpdateDropdown();
        }
    }

    private void UpdateDropdown()
    {
        List<string> playerIDs = new List<string>();
        for (int i = 0; i < clientListData.clients.Count; ++i)
        {
            playerIDs.Add("Player: " + clientListData.clients[i].ToString());
        }

        playerListDropdown.ClearOptions();
        playerListDropdown.AddOptions(playerIDs);

        Debug.LogError("Client List Updated");

        updateRequired = false;
    }
}
