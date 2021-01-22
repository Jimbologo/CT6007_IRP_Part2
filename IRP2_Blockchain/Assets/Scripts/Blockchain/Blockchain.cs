using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Blockchain
{
    public List<Block> theBlockchain = new List<Block>();

    public Blockchain(List<Block> a_theBlockchain)
    {
        theBlockchain = a_theBlockchain;
    }

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
