using Project.Scripts.Runtime.utils;
using UnityEngine;

namespace Project.Scripts.Runtime.effect.Dirt
{
    public class Dirt : MonoBehaviour
    {
        [SerializeField] private GameObject dirtParticle;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

        }
    
        // 当たった時に呼ばれる関数
        void OnCollisionEnter(Collision collision)
        {
            Instantiate(dirtParticle).Apply(it =>
            {
                it.transform.position = collision.transform.position;
                it.transform.rotation = new Quaternion(0,0,0,0);
                it.transform.parent = this.transform;
                it.SetActive(true);
            });
        }
    }
}
