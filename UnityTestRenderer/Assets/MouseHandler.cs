using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class MouseHandler : MonoBehaviour {

	
	
    public Transform selectionCube;
	
    void Start() {
    }

    // Update is called once per frame
    void Update () {
		
		
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hitInfo;
            Debug.Log ("Click!");
            
            if( GetComponent<Collider>().Raycast( ray, out hitInfo, Mathf.Infinity ) ) {
                int x = Mathf.FloorToInt( hitInfo.point.x / StartUp.TileSize);
                int y = Mathf.FloorToInt( hitInfo.point.y / StartUp.TileSize);
                //Debug.Log ("Tile: " + x + ", " + z);
                Debug.Log ($"Clicked at tile x={x}. y={y}");
            }
            else {
                Debug.Log ("clicked outside of mesh");
            }
        }
    }
}
