using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

enum DataType
{
    stringType,
    intType,
    blockType,
    blockchainType,
    clientList
}

/// <summary>
/// Handles Network Data to write and read
/// </summary>
public class NET_HandleData : MonoBehaviour
{

    /// <summary>
    /// Writes string data
    /// </summary>
    /// <param name="a_stringMessage"></param>
    /// <returns></returns>
    public static byte[] WriteData(string a_stringMessage)
    {
        //When we write data we always put a key before the data, this way we know what type of data we are sending
        byte[] combinedData = new byte[NET_Constants.packetSize];

        //add key to the data in first 4 bytes
        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.stringType);
        dataKey.CopyTo(combinedData, 0);

        //Add message to the array
        byte[] byteMsg = Encoding.ASCII.GetBytes(a_stringMessage);
        byteMsg.CopyTo(combinedData, 4);

        //add message length to the end of the array so we know how much of the array the message is contained in
        byte[] byteMsgLength = BitConverter.GetBytes(byteMsg.Length);
        byteMsgLength.CopyTo(combinedData, NET_Constants.packetSize - sizeof(int));

        return combinedData;
    }

    /// <summary>
    /// Writes Int Data
    /// </summary>
    /// <param name="a_intMessage"></param>
    /// <returns></returns>
    public static byte[] WriteData(int a_intMessage)
    {
        //When we write data we always put a key before the data, this way we know what type of data we are sending
        byte[] combinedData = new byte[NET_Constants.packetSize];

        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.intType);
        dataKey.CopyTo(combinedData, 0);

        byte[] byteMsg = BitConverter.GetBytes(a_intMessage);
        byteMsg.CopyTo(combinedData, 4);

        byte[] byteMsgLength = BitConverter.GetBytes(byteMsg.Length);
        byteMsgLength.CopyTo(combinedData, NET_Constants.packetSize - sizeof(int));

        return combinedData;
    }


    /// <summary>
    /// Writes List<NET_ConnectedClient> data
    /// </summary>
    /// <param name="a_connectedClients"></param>
    /// <returns></returns>
    public static byte[] WriteData(List<int> a_connectedClients)
    {
        //When we write data we always put a key before the data, this way we know what type of data we are sending
        byte[] combinedData = new byte[NET_Constants.packetSize];

        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.clientList);
        dataKey.CopyTo(combinedData, 0);


        NET_ClientList clientList = new NET_ClientList(a_connectedClients);
        byte[] byteMsg = clientList.GetBytes();
        byteMsg.CopyTo(combinedData, 4);

        byte[] byteMsgLength = BitConverter.GetBytes(byteMsg.Length);
        byteMsgLength.CopyTo(combinedData, NET_Constants.packetSize - sizeof(int));

        return combinedData;
    }

    /// <summary>
    /// Writes Block data
    /// </summary>
    /// <param name="a_blockMessage"></param>
    /// <returns></returns>
    public static byte[] WriteData(Block a_blockMessage)
    {
        //When we write data we always put a key before the data, this way we know what type of data we are sending
        byte[] combinedData = new byte[NET_Constants.packetSize];

        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.blockType);
        dataKey.CopyTo(combinedData, 0);

        byte[] byteMsg = a_blockMessage.GetBytes();
        byteMsg.CopyTo(combinedData, 4);

        byte[] byteMsgLength = BitConverter.GetBytes(byteMsg.Length);
        byteMsgLength.CopyTo(combinedData, NET_Constants.packetSize - sizeof(int));

        return combinedData;
    }

    /// <summary>
    /// Writes Blockchain data
    /// </summary>
    /// <param name="a_blockchainMessage"></param>
    /// <returns></returns>
    public static byte[] WriteData(Blockchain a_blockchainMessage)
    {
        //When we write data we always put a key before the data, this way we know what type of data we are sending
        byte[] combinedData = new byte[NET_Constants.packetSize];

        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.blockchainType);
        dataKey.CopyTo(combinedData, 0);

        byte[] byteMsg = a_blockchainMessage.GetBytes();
        byteMsg.CopyTo(combinedData, 4);

        byte[] byteMsgLength = BitConverter.GetBytes(byteMsg.Length);
        byteMsgLength.CopyTo(combinedData, NET_Constants.packetSize - sizeof(int));

        return combinedData;
    }

    /// <summary>
    /// Reads data in and seperates into correct handling method
    /// </summary>
    /// <param name="a_byteArray"></param>
    /// <param name="a_networkManager"></param>
    public static void ReadData(byte[] a_byteArray, NET_NetworkManager a_networkManager)
    {
        //when reading, we read a set amount of first bytes so we know what type of data we are reading
        byte[] dataKey = new byte[4];
        Array.Copy(a_byteArray, dataKey, 4);

        DataType dataType = (DataType)BitConverter.ToInt32(dataKey, 0);

        //Get just the message from the data
        byte[] msgLengthBytes = new byte[4];
        Array.Copy(a_byteArray, NET_Constants.packetSize - sizeof(int), msgLengthBytes, 0, sizeof(int));
        int msgLength = BitConverter.ToInt32(msgLengthBytes, 0);

        byte[] msgBytes = new byte[msgLength];
        Array.Copy(a_byteArray, sizeof(int), msgBytes, 0, msgLength);

        //Pass message to the correct handle function
        switch (dataType)
        {
            case DataType.stringType:
                HandleStringData(msgBytes);
                break;
            case DataType.intType:
                HandleIntData(msgBytes, a_networkManager);
                break;
            case DataType.clientList:
                HandleClientListData(msgBytes, a_networkManager);
                break;
            case DataType.blockType:
                HandleBlockData(msgBytes, a_networkManager);
                break;
            case DataType.blockchainType:
                HandleBlockchainData(msgBytes, a_networkManager);
                break;
            default:
                Debug.LogWarning("Could not find Data Type of message");
                break;
        }
    }

    /// <summary>
    /// Read String data
    /// </summary>
    /// <param name="a_data"></param>
    private static void HandleStringData(byte[] a_data)
    {
        string dataConverted = Encoding.ASCII.GetString(a_data);
        Debug.Log("New String Recieved: " + dataConverted);
    }

    /// <summary>
    /// Reads Int Data
    /// </summary>
    /// <param name="a_data"></param>
    /// <param name="a_networkManager"></param>
    private static void HandleIntData(byte[] a_data, NET_NetworkManager a_networkManager)
    {
        int dataConverted = BitConverter.ToInt32(a_data, 0);
        Debug.Log("New ID Recieved: " + dataConverted);

        //forwards data to networkmanager to handle
        a_networkManager.myID = dataConverted;
        a_networkManager.UpdatePlayerData();
    }

    /// <summary>
    /// Reads Block data
    /// </summary>
    /// <param name="a_data"></param>
    /// <param name="a_networkManager"></param>
    private static void HandleBlockData(byte[] a_data, NET_NetworkManager a_networkManager)
    {
        Block dataConverted = Block.ConvertBytes(a_data);

        Debug.Log("New Block Recieved with hash: " + Encoding.ASCII.GetString(dataConverted.GetCurrentBlockHash()));

        //forwards data to networkmanager to handle
        a_networkManager.HandleBlockData(dataConverted);
    }

    /// <summary>
    /// Reads Blockchain Data
    /// </summary>
    /// <param name="a_data"></param>
    /// <param name="a_networkManager"></param>
    private static void HandleBlockchainData(byte[] a_data, NET_NetworkManager a_networkManager)
    {
        Blockchain dataConverted = Blockchain.ConvertBytes(a_data);

        Debug.Log("Updated Blockchain Recieved with " + dataConverted.theBlockchain.Count + " Blocks");

        //forwards data to networkmanager to handle
        a_networkManager.HandleBlockchainData(dataConverted);
    }

    /// <summary>
    /// Reads Client List Data
    /// </summary>
    /// <param name="a_data"></param>
    /// <param name="a_networkManager"></param>
    private static void HandleClientListData(byte[] a_data, NET_NetworkManager a_networkManager)
    {
        NET_ClientList dataConverted = NET_ClientList.ConvertBytes(a_data);

        Debug.Log("Updated Client List Recieved");

        //forwards data to networkmanager to handle
        a_networkManager.HandleClientListData(dataConverted);
    }


}
