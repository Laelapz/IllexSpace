using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShootScript : MonoBehaviour
{
    public float timeToDestroy = 1f;
    public float speed = 1f;
    public string type = "player";
    public SpriteRenderer shootColor;
    public ParticleSystem shootParticleColor;

    public UnityAction<Vector3, GameObject, Transform> callback;
    public int dir = 1;

    private void Start()
    {
        var particleSystem = shootParticleColor.main;

        if(shootColor) particleSystem.startColor = shootColor.color;
        StartCoroutine(callbackBeforeDestroy());
        Destroy(gameObject, timeToDestroy);
    }

    void Update() {
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3 ( 0, speed * Time.deltaTime );
        pos += transform.rotation * velocity;
        transform.position = pos;
    }

    void OnTriggerEnter2D ()
    {
        if (callback != null) callback.Invoke(transform.position, gameObject, transform.parent);
        Destroy(gameObject);
    }

    IEnumerator callbackBeforeDestroy()
    {
        yield return new WaitForSeconds(timeToDestroy-0.1f);
        if (callback != null) callback.Invoke(transform.position, gameObject, transform.parent);

    }
}
