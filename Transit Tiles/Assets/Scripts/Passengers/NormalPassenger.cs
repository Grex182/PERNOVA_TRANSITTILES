using System.Collections.Generic;
using UnityEngine;

public class NormalPassenger : Passenger
{
    public override List<Vector2Int> GetAvailableMoves(ref Passenger[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        //Down
        for (int i = currentY - 1; i >= 0; i--)
        {
            if (board[currentX, i] == null)
            {
                r.Add(new Vector2Int(currentX, i));
            }

            if (board[currentX, i] != null)
            {
                break;
            }
        }

        //Up
        for (int i = currentY + 1; i < tileCountY; i++)
        {
            if (board[currentX, i] == null)
            {
                r.Add(new Vector2Int(currentX, i));
            }

            if (board[currentX, i] != null)
            {
                break;
            }
        }

        //Left
        for (int i = currentX - 1; i >= 0; i--)
        {
            if (board[i, currentY] == null)
            {
                r.Add(new Vector2Int(i, currentY));
            }

            if (board[i, currentY] != null)
            {
                break;
            }
        }

        //Right
        for (int i = currentX + 1; i < tileCountX; i++)
        {
            if (board[i, currentY] == null)
            {
                r.Add(new Vector2Int(i, currentY));
            }

            if (board[i, currentY] != null)
            {
                break;
            }
        }

        return r;
    }
}
