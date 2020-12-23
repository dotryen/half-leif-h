using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour {
    public GameObject mesh;
    public AudioSource source;
    public AudioClip[] sounds;
    public ParticleSystem particle;
    public float radius;
    public float force;

    public void Explode() {
        mesh.SetActive(false);

        var colliders = Physics.OverlapSphere(transform.position, radius);
        var explosives = new List<ExplosiveBarrel>();

        foreach (Collider col in colliders) {
            var ex = col.GetComponentInParent<ExplosiveBarrel>();
            if (ex) {
                ex.Explode();
                continue;
            }

            if (col.attachedRigidbody) {
                col.attachedRigidbody.AddExplosionForce(force, transform.position, radius, 1, ForceMode.VelocityChange);
            }
        }
        
        particle.Play();
        source.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);

        Invoke(nameof(Die), 3);
    }

    private void Die() {
        Destroy(gameObject);
    }
}
