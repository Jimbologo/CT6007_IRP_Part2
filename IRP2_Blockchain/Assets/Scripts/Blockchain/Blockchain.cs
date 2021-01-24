using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Holds list of blocks ready for sending over network
/// </summary>
[System.Serializable]
public class Blockchain
{
    public List<Block> theBlockchain = new List<Block>();

    public Blockchain(List<Block> a_theBlockchain)
    {
        theBlockchain = a_theBlockchain;
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
            return ms.ToArray();
        }
    }

    /// <summary>
    /// Takes bytes and convert back into Object
    /// </summary>
    /// <param name="a_bytes"></param>
    /// <returns></returns>
    public static Blockchain ConvertBytes(byte[] a_bytes)
    {
        //Calculate total size of the byte array
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(a_bytes, 0, a_bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Blockchain obj = (Blockchain)binForm.Deserialize(memStream);
            return obj;
        }

    }
}
