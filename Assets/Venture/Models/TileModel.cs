using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModel
{
    CityModel city;
    Vector3 position;
    bool isOccupied = false;

    public TileModel(CityModel city, Vector3 position)
    {
        this.city = city;
        this.position = position;
    }
}
