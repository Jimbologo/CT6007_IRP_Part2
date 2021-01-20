using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

enum DataType
{
    stringType,
    intType,
    transactionType
}

public class NET_HandleData
{
    //When we write data we always put a key before the data, this way we know what type of data we are sending
    //We are writing string data
    public byte[] WriteData(string a_stringMessage)
    {
        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.stringType);

        byte[] combinedData = new byte[4096];
        dataKey.CopyTo(combinedData, 0);

        byte[] byteMsg = Encoding.ASCII.GetBytes(a_stringMessage);
        byteMsg.CopyTo(combinedData, 4);

        return combinedData;
    }

    //When we write data we always put a key before the data, this way we know what type of data we are sending
    //We are writing string data
    public byte[] WriteData(int a_intMessage)
    {
        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.stringType);

        byte[] combinedData = new byte[4096];
        dataKey.CopyTo(combinedData, 0);

        byte[] byteMsg = BitConverter.GetBytes(a_intMessage);
        byteMsg.CopyTo(combinedData, 4);

        return combinedData;
    }

    //When we write data we always put a key before the data, this way we know what type of data we are sending
    //We are writing string data
    public byte[] WriteData(Transaction a_transactionMessage)
    {
        byte[] dataKey = new byte[4];
        dataKey = BitConverter.GetBytes((int)DataType.stringType);

        byte[] combinedData = new byte[4096];
        dataKey.CopyTo(combinedData, 0);

        byte[] byteMsg = a_transactionMessage.GetBytes();
        byteMsg.CopyTo(combinedData, 4);

        return combinedData;
    }

    //when reading, we read a set amount of first bytes so we know what type of data we are reading
    public void ReadData(byte[] a_byteArray)
    {
        byte[] dataKey = new byte[4];
        Array.Copy(a_byteArray, dataKey, 4);

        DataType dataType = (DataType)BitConverter.ToInt32(dataKey, 0);

        switch(dataType)
        {
            case DataType.stringType:

                break;
            case DataType.intType:

                break;
            case DataType.transactionType:

                break;
            default:
                Debug.LogWarning("Could not find Data Type of message");
                break;
        }
    }
}
