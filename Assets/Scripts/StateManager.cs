using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class StateManager : Singleton<StateManager>{
    public int maxObstacles;
    [Space]
    public GameObject player;
    public GameObject collectablePrefab;
    public GameObject obstaclesRoot;
    public GameObject obstacle;
    [Space]
    [Header("Spawn Area")]
    public Vector3 playerVolume;
    public Vector3 collectablesVolume;
    public Vector3 obstaclesVolume;

    public Observable<int> currentScore = new Observable<int>(0);
    public Observable<int> currentShoots = new Observable<int>(0);
    public Observable<int> currentMiss = new Observable<int>(0);

    public Shooter Agent;

    int maxIterationsSpawn = 10;

    // Increment the score when ball touches the goal. 
    public void addPoint(){
        currentScore.val ++;
    }

    // Increment the number of time player threw the ball. 
    public void shoot(){
        currentShoots.val ++;
    }

    void Start(){
        currentScore.propertyUpdated += onScore;

        movePlayer();
        createObstacles();
        createNewCollectable();

        // Removed for AI training
        // PredictionManager.instance.copyAllObstacles();

        Agent.OnEnvironmentReset += Respawn;
    }

    // Monitor if the ball misses the goal.
    // TODO: Change it so it doesn't go into infinity loop.
    void Update()
    {
        if(currentShoots.val > currentScore.val)
        {
            currentMiss.val = currentShoots.val - currentScore.val;
            // Debug.Log(currentShoots.val);
            // Debug.Log(currentScore.val);
        }
    }

    // Destroy and Inititae a new random Goal every time we score.
    void onScore(int v){
        Destroy(GameObject.FindWithTag("Goal"));
        createNewCollectable();
    }

    void movePlayer(){
        player.transform.position = calculatePositionInVolume(playerVolume);
    }

    // Initiate a new Goal.
    void createNewCollectable(){
        GameObject c = Instantiate(collectablePrefab);
        bool empty = false;
        int iteration = 0;

        Vector3 p = Vector3.zero;

        while(!empty && iteration < maxIterationsSpawn){
            p = calculatePositionInVolume(collectablesVolume);
            var hits = Physics.OverlapSphere(p, .6f);
            empty = hits.Length <= 0;
            iteration ++;
        }

        c.transform.position = p;
    }

    // Inititate new obstacles.
    // TODO: Add movement to the plane.
    void createObstacles(){
        int currentObs = 0;
        while(currentObs < maxObstacles){
            GameObject o = Instantiate(obstacle);
            Collider oc = o.GetComponent<Collider>();
            bool empty = false;
            int iterations = 0;

            Vector3 p = Vector3.zero;
            Quaternion r = Random.rotation;

            while(!empty && iterations < maxIterationsSpawn){
                p = calculatePositionInVolume(obstaclesVolume);
                var hits = Physics.OverlapBox(p, oc.bounds.extents, r);
                empty = hits.Length <= 0;
                iterations ++;
            }

            o.transform.position = p;
            o.transform.rotation = r;
            o.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            o.transform.SetParent(obstaclesRoot.transform);
            currentObs ++;
        }
    }

    // Calculate a random position inside the Gizmos
    Vector3 calculatePositionInVolume(Vector3 vol){
        Vector3 p = new Vector3();
        p.x = Random.Range(-vol.x/2, vol.x/2);
        p.y = Random.Range(-vol.y/2, vol.y/2);
        p.z = Random.Range(-vol.z/2, vol.z/2);
        return transform.position + transform.rotation * p;
    }

    void OnDrawGizmos() {
		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(Vector3.zero, collectablesVolume);

        Gizmos.color = Color.green;
		Gizmos.DrawWireCube(Vector3.zero, playerVolume);

        Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(Vector3.zero, obstaclesVolume);
	}

    public void Respawn()
    {
        currentScore.val = 0;
        currentShoots.val = 0;
    }
}
