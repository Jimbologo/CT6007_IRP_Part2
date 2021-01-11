using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public class Block
{
    private Transaction[] transactions;

    private byte[] previousBlockHash;
    private byte[] currentBlockHash;

    HashAlgorithm algorithm;

    //Constructor
    public Block(byte[] a_previousBlockHash, Transaction[] a_transactions)
    {
        transactions = a_transactions;
        previousBlockHash = a_previousBlockHash;

        algorithm = MD5.Create();

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

    //Convert Transaction Object to Byte Hash
    public static byte[] GetTransactionBytes(Transaction a_newTransaction)
    {

        byte[] currentUserIDBytes = BitConverter.GetBytes(a_newTransaction.getCurrentUserID());
        byte[] targetUserIDBytes = BitConverter.GetBytes(a_newTransaction.getTargetUserID());
        byte[] actionTakenBytes = Encoding.UTF8.GetBytes(a_newTransaction.getActionTaken());

        List<byte> byteList = new List<byte>();
        byteList.AddRange(currentUserIDBytes);
        byteList.AddRange(targetUserIDBytes);
        byteList.AddRange(actionTakenBytes);

        return byteList.ToArray();
    }

    //Convert Transactions Array to Byte Hash
    public static byte[] GetTransactionArrayHash(Transaction[] a_newTransactions)
    {
        HashAlgorithm algorithm = MD5.Create();
        List<byte> byteList = new List<byte>();

        for (int i = 0; i < a_newTransactions.Length; ++i)
        {
            byte[] bytesOfTransaction = GetTransactionBytes(a_newTransactions[i]);
            byteList.AddRange(bytesOfTransaction);
        }

        return algorithm.ComputeHash(byteList.ToArray());
    }
}
