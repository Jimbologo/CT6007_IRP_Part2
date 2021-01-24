using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Holds an array of all transactions with hashes for both last and this block
/// </summary>
[System.Serializable]
public class Block
{
    private Transaction[] transactions;

    private byte[] previousBlockHash;
    private byte[] currentBlockHash;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="a_previousBlockHash"></param>
    /// <param name="a_transactions"></param>
    public Block(byte[] a_previousBlockHash, Transaction[] a_transactions)
    {
        transactions = a_transactions;
        previousBlockHash = a_previousBlockHash;

        HashAlgorithm algorithm = MD5.Create();

        //Calculate the transactions Hash
        CalculateHash();
    }

    /// <summary>
    /// Gets the previous blocks hash
    /// </summary>
    /// <returns></returns>
    public byte[] GetPreviousBlockHash()
    {
        return previousBlockHash;
    }

    /// <summary>
    /// Gets the current blocks hash
    /// </summary>
    /// <returns></returns>
    public byte[] GetCurrentBlockHash()
    {
        return currentBlockHash;
    }

    /// <summary>
    /// Gets this blocks array of transactions
    /// </summary>
    /// <returns></returns>
    public Transaction[] GetTransactions()
    {
        return transactions;
    }

    /// <summary>
    /// Calculates this blocks Hash
    /// </summary>
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

    /// <summary>
    /// Calculate this blocks Hash and returns
    /// </summary>
    /// <returns></returns>
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
            Debug.LogWarning("Calculated Hash is null, This probably means the transactions hashed was empty/Null");
        }

        return thisHash;
    }

    /// <summary>
    /// Calculate this blocks Hash from a specified previous blocks hash and returns
    /// </summary>
    /// <param name="a_transactions"></param>
    /// <param name="a_previousBlockHash"></param>
    /// <returns></returns>
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
            Debug.LogWarning("Calculated Hash is null, This probably means the transactions hashed was empty/Null");
        }

        return thisHash;
    }



    /// <summary>
    /// Convert Transactions Array to Byte Hash
    /// </summary>
    /// <param name="a_newTransactions"></param>
    /// <returns></returns>
    public static byte[] GetTransactionArrayHash(Transaction[] a_newTransactions)
    {
        HashAlgorithm algorithm = MD5.Create();
        List<byte> byteList = new List<byte>();

        //Add bytes of all transaction into one array
        for (int i = 0; i < a_newTransactions.Length; ++i)
        {
            byte[] bytesOfTransaction = a_newTransactions[i].GetBytes();
            byteList.AddRange(bytesOfTransaction);
        }

        //Computer the hash of these transactions
        return algorithm.ComputeHash(byteList.ToArray());
    }

    /// <summary>
    /// Converts Object into Bytes and returns
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Takes bytes and convert back into Object
    /// </summary>
    /// <param name="a_bytes"></param>
    /// <returns></returns>
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
