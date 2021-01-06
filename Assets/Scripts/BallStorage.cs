using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallStorage : MonoBehaviour
{

    public GameObject BallPrefab;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            // Instantiate a new copy of the ball prefab
            GameObject playerBall = Instantiate( BallPrefab );

            AddBallToStorage(playerBall , this.transform.GetChild(i) );
        }
    }

    public void AddBallToStorage( GameObject playerBall, Transform thePlaceholder=null )
    {
        if( thePlaceholder == null )
        {
            // Find the first empty placeholder.
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Transform p = this.transform.GetChild(i);
                if( p.childCount == 0 )
                {
                    // This placeholder is empty!
                    thePlaceholder = p;
                    break;  // Break out of the loop.
                }
            }

            if(thePlaceholder==null)
            {
                Debug.LogError("We're trying to add a stone but we don't have empty places. How did this happen?!?!?");
                return;
            }
        }

        // Parent the stone to the placeholder
        playerBall.transform.SetParent( thePlaceholder );

        // Reset the stone's local position to 0,0,0
        playerBall.transform.localPosition = Vector3.zero;
    }
}
