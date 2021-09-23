using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticleSystem: MonoBehaviour {

    public List<GameObject> objs;

    private List<ParticleSystem> ps;

    void Start() {}

    private void Awake() {
        foreach(GameObject o in objs)
            ps.Add(o.GetComponent < ParticleSystem > ());
    }

    private void Update() {

        if (ps.Count > 0) {
            foreach (ParticleSystem o in ps) {
                if ( !o.IsAlive()) {
                    Destroy(o.gameObject);
                }
            }
        }
    }
}