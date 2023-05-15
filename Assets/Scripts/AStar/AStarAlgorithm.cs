using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    PriorityQueue OpenQueue = new PriorityQueue();
    List<Node> VisitedNodes = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Once Ian is done with his procedural generation, retrieve a reference to the collection of tile objects in his script.
        List<GameObject> mazeFloor = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
