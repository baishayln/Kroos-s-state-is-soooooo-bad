using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField]private ParticleSystem particleSst;
    private float timer;
    private float lifetime = 5;
    // Start is called before the first frame update
    void Start()
    {
        if (!particleSst)
        {
            particleSst = transform.GetComponent<ParticleSystem>();
        }
    }
    void OnEnable()
    {
        // particleSst.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            if (timer <= Time.deltaTime)
            {
                particleSst.Stop();
                ObjectPool.Instance.PushObject(gameObject);
            }
            timer -= Time.deltaTime;
        }
    }
    public void SetData(Vector3 position)
    {
        transform.position = position;
        particleSst.Play();
        timer = lifetime;
    }
}
