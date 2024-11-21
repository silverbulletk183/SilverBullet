using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData:IEquatable<PlayerData>, INetworkSerializable
{
    // Start is called before the first frame update
    public ulong clientId;
    public FixedString64Bytes playerName;
    public FixedString64Bytes userId;
    public bool Equals (PlayerData other)
    {
        return clientId == other.clientId&& playerName == other.playerName && userId == other.userId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref userId);
    }
}
