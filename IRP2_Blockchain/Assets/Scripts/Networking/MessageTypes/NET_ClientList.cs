using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Holds List of client ID
/// </summary>
[System.Serializable]
public class NET_ClientList 
{
    public List<int> clients;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="a_clients"></param>
    public NET_ClientList(List<int> a_clients)
    {
        clients = a_clients;
    }

    /// <summary>
    /// Convert Object into Byte array and returns
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
    /// Convert byte array into Object and return
    /// </summary>
    /// <param name="a_bytes"></param>
    /// <returns></returns>
    public static NET_ClientList ConvertBytes(byte[] a_bytes)
    {
        //Calculate total size of the byte array
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(a_bytes, 0, a_bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            NET_ClientList obj = (NET_ClientList)binForm.Deserialize(memStream);
            return obj;
        }
    }

}
