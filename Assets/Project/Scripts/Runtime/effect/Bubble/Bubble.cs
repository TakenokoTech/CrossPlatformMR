using UnityEngine;

namespace Project.Scripts.Runtime.effect.Bubble
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] internal bool emit = false;
        [SerializeField] private GameObject particle;
        
        private MeshRenderer mesh = null;
        private GameObject cam = null;
        private float randFloat = 0;
        
        void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            if (Camera.main != null) cam = Camera.main.gameObject;
            randFloat = Random.Range(-10.0f, 10.0f);
        }

        void Update()
        {
            if (cam != null)
                transform.LookAt(cam.transform);

            var pos = transform.position;
            transform.position =
                new Vector3(pos.x, pos.y + 0.001F * Mathf.Sin(Time.frameCount * 0.1F + randFloat), pos.z);

            if (emit)
            {
                mesh.enabled = false;
                particle.SetActive(true);
            }
            else
            {
                mesh.enabled = true;
                particle.SetActive(false);                
            }
        }
    }
}