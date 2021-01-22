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
    blockchainType
}

public class NET_HandleData : MonoBehaviour
{

    //When we write data we always put a key before the data, this way we know what type of data we are sending
    //We are writing string data
    public static byte[] WriteData(string a_stringMessage)
    {
        byte[] combinedData = new byte[NET_Constants.packetSize];

        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.stringType);
        dataKey.CopyTo(combinedData, 0);

        byte[] byteMsg = Encoding.ASCII.GetBytes(a_stringMessage);
        byteMsg.CopyTo(combinedData, 4);

        byte[] byteMsgLength = BitConverter.GetBytes(byteMsg.Length);
        byteMsgLength.CopyTo(combinedData, NET_Constants.packetSize - sizeof(int));

        return combinedData;
    }

    //When we write data we always put a key before the data, this way we know what type of data we are sending
    //We are writing int data
    public static byte[] WriteData(int a_intMessage)
    {
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

    //When we write data we always put a key before the data, this way we know what type of data we are sending
    //We are writing block data
    public static byte[] WriteData(Block a_blockMessage)
    {
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

    //When we write data we always put a key before the data, this way we know what type of data we are sending
    //We are writing blockchain data
    public static byte[] WriteData(Blockchain a_blockchainMessage)
    {
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

    //when reading, we read a set amount of first bytes so we know what type of data we are reading
    public static void ReadData(byte[] a_byteArray, NET_NetworkManager a_networkManager)
    {
        byte[] dataKey = new byte[4];
        Array.Copy(a_byteArray, dataKey, 4);

        DataType dataType = (DataType)BitConverter.ToInt32(dataKey, 0);

        //Get just the message from the data
        byte[] msgLengthBytes = new byte[4];
        Array.Copy(a_byteArray, NET_Constants.packetSize - sizeof(int), msgLengthBytes, 0, sizeof(int));
        int msgLength = BitConverter.ToInt32(msgLengthBytes, 0);

        byte[] msgBytes = new byte[msgLength];
        Array.Copy(a_byteArray, sizeof(int), msgBytes, 0, msgLength);

        switch (dataType)
        {
            case DataType.stringType:
                HandleStringData(msgBytes);
                break;
            case DataType.intType:
                HandleIntData(msgBytes);
                break;
            case DataType.blockType:
                HandleBlockData(msgBytes, a_networkManager);
                break;
            case DataType.blockchainType:
                HandleBlockchainData(msgBytes, a_networkManager);
                break;
            default:
                Debug.LogError("Could not find Data Type of message");
                break;
        }
    }

    private static void HandleStringData(byte[] a_data)
    {
        string dataConverted = Encoding.ASCII.GetString(a_data);
        Debug.LogError("New String Recieved: " + dataConverted);
    }

    private static void HandleIntData(byte[] a_data)
    {
        int dataConverted = BitConverter.ToInt32(a_data, 0);
        Debug.LogError("New Int Recieved: " + dataConverted);
    }

    private static void HandleBlockData(byte[] a_data, NET_NetworkManager a_networkManager)
    {
        Block dataConverted = Block.ConvertBytes(a_data);

        Debug.LogError("New Block Recieved with hash: " + Encoding.ASCII.GetString(dataConverted.GetCurrentBlockHash()));

        a_networkManager.HandleBlockData(dataConverted);
    }

    private static void HandleBlockchainData(byte[] a_data, NET_NetworkManager a_networkManager)
    {
        Blockchain dataConverted = Blockchain.ConvertBytes(a_data);

        Debug.LogError("Updated Blockchain Recieved with " + dataConverted.theBlockchain.Count + " Blocks");

        a_networkManager.HandleBlockchainData(dataConverted);
    }
}
