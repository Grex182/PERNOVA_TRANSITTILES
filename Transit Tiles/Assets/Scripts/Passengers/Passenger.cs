using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassengerType
{
    None = 0,
    Standard = 1,
    Elder = 2,
    Knight = 3,
    Bishop = 4,
    Queen = 5,
    King = 6
}

public class Passenger : MonoBehaviour
{
    public int currentX;
    public int currentY;
    public PassengerType type;

    private const string ColorProperty = "_BaseColor";
    private string randomColor;

    private Vector3 desiredPosition;
    //[SerializeField] private Vector3 desiredScale = Vector3.one;

    private bool isInsideTrain = false;

    private void Start()
    {
        randomColor = validStationColors[Random.Range(0, validStationColors.Length)];

        Debug.Log("Random Color: " + randomColor);

        SetPassengerStation(gameObject, randomColor);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        //transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
    }

    public virtual List<Vector2Int> GetAvailableMoves(ref Passenger[,] board, int tileCountX, int tileCountY)
    {
        //r means return value
        List<Vector2Int> r = new List<Vector2Int>();

        r.Add(new Vector2Int(3, 3));
        r.Add(new Vector2Int(3, 4));
        r.Add(new Vector2Int(4, 3));
        r.Add(new Vector2Int(4, 4));

        return r;
    }

    public virtual void SetPosition(Vector3 position, bool force = false)
    {
        desiredPosition = position;

        if (force)
        {
            transform.position = desiredPosition;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrainTile") && !isInsideTrain)
        {
            isInsideTrain = true;

            Debug.Log("Passenger entered train.");
        }
        else if (other.CompareTag("ExitTile") && isInsideTrain)
        {
            StationManager.instance.CheckPassengerStation();
            isInsideTrain = false;

            Debug.Log("Passenger exited train.");
        }
    }

    private bool SetPassengerStation(GameObject passenger, string stationColor)
    {
        //could be changed to enum instead, but for now, its by gameObject name
        if (gameObject.name.Contains("Girl"))
        {
            Transform childTransform = passenger.transform.Find("Torso");

            MeshRenderer childMeshRenderer = childTransform.GetComponent<MeshRenderer>();

            var material = childMeshRenderer.material;
            material.SetColor(ColorProperty, GetStationColor(stationColor));
            return true;
        }

        #region NULL-CHECKS
        if (!passenger.TryGetComponent<MeshRenderer>(out var meshRenderer))
        {
            Debug.LogError("No MeshRenderer found on passenger prefab.");
            return false;
        }
        #endregion

        return true;
    }

    private static readonly string[] validStationColors = new string[]
    {
        "Pink", "Red", "Orange", "Yellow", "Green", "Blue", "Violet"
    };

    private Color GetStationColor(string stationColor)
    {
        switch (stationColor)
        {
            case "Pink": return Color.magenta;
            case "Red": return Color.red;
            case "Orange": return new Color(1f, 0.5f, 0f);
            case "Yellow": return Color.yellow;
            case "Green": return Color.green;
            case "Blue": return Color.blue;
            case "Violet": return new Color(0.5f, 0f, 1f);
            default: return Color.white;
        }
    }
}
