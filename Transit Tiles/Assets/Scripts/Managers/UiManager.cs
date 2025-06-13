using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> stars = new List<GameObject>(); // Public Rating

    [Header("Timer References")]
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI currPhase;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
