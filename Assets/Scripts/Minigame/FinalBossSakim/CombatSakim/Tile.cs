using System;
using UnityEngine;

public enum TileType { Empty, Energy, Damage }

public class Tile : MonoBehaviour
{
    public TileType tileType;
}
