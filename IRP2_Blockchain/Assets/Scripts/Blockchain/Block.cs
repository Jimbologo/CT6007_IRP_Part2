using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Block
{
    private Transaction[] transactions;

    private byte[] previousBlockHash;
    private byte[] currentBlockHash;


    //Constructor
    public Block(byte[] a_previousBlockHash, Transaction[] a_transactions)
    {
        transactions = a_transactions;
        previousBlockHash = a_previousBlockHash;

        HashAlgorithm algorithm = MD5.Create();

        //Calculate the transactions Hash
        CalculateHash();
    }

    //Getter
    public byte[] GetPreviousBlockHash()
    {
        return previousBlockHash;
    }

    //Getter
    public byte[] GetCurrentBlockHash()
    {
        return currentBlockHash;
    }

    //Getter
    public Transaction[] GetTransactions()
    {
        return transactions;
    }

    //Calculate the transactions Hash
    public void CalculateHash()
    {
        HashAlgorithm algorithm = MD5.Create();

        byte[] transactionsHashed = GetTransactionArrayHash(transactions);

        //Combine previous hash with this hash to provide the security, Then Hash That
        List<byte> newByteList = new List<byte>();
        newByteList.AddRange(transactionsHashed);
        newByteList.AddRange(previousBlockHash);

        currentBlockHash = algorithm.ComputeHash(newByteList.ToArray());
    }

    //Calculate the transactions Hash then return
    public byte[] CalculateHashReturned()
    {
        HashAlgorithm algorithm = MD5.Create();

        byte[] transactionsHashed = GetTransactionArrayHash(transactions);
        //Combine previous hash with this hash to provide the security, Then Hash That
        List<byte> newByteList = new List<byte>();
        newByteList.AddRange(transactionsHashed);
        newByteList.AddRange(previousBlockHash);

        byte[] thisHash = algorithm.ComputeHash(newByteList.ToArray());

        //Check hash is valid
        if (thisHash == null)
        {
            Debug.LogError("Calculated Hash is null, This probably means the transactions hashed was empty/Null");
        }

        return thisHash;
    }

    //Calculate the transactions Hash then return
    public static byte[] CalculateHashReturned(Transaction[] a_transactions, byte[] a_previousBlockHash)
    {
        HashAlgorithm algorithm = MD5.Create();

        byte[] transactionsHashed = GetTransactionArrayHash(a_transactions);
        //Combine previous hash with this hash to provide the security, Then Hash That
        List<byte> newByteList = new List<byte>();
        newByteList.AddRange(transactionsHashed);
        newByteList.AddRange(a_previousBlockHash);

        byte[] thisHash = algorithm.ComputeHash(newByteList.ToArray());

        //Check hash is valid
        if (thisHash == null)
        {
            Debug.LogError("Calculated Hash is null, This probably means the transactions hashed was empty/Null");
        }

        return thisHash;
    }



    //Convert Transactions Array to Byte Hash
    public static byte[] GetTransactionArrayHash(Transaction[] a_newTransactions)
    {
        HashAlgorithm algorithm = MD5.Create();
        List<byte> byteList = new List<byte>();

        for (int i = 0; i < a_newTransactions.Length; ++i)
        {
            byte[] bytesOfTransaction = a_newTransactions[i].GetBytes();
            byteList.AddRange(bytesOfTransaction);
        }

        return algorithm.ComputeHash(byteList.ToArray());
    }

    public byte[] GetBytes()
    {
        //Calculate total size of the byte array
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            byte[] bytes = ms.ToArray();
            return bytes;
        }
    }

    public static Block ConvertBytes(byte[] a_bytes)
    {
        //Calculate total size of the byte array
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(a_bytes, 0, a_bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Block obj = (Block)binForm.Deserialize(memStream);
            return obj;
        }
    }
}
