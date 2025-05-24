using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassengerType
{
    None = 0,
    Pawn = 1,
    Rook = 2,
    Knight = 3,
    Bishop = 4,
    Queen = 5,
    King = 6
}

public class Passenger : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public PassengerType type;

    private Vector3 desiredPosition;
    private Vector3 desiredScale;
}
