using UnityEngine;

namespace Project.Scripts.Runtime.effect.Temp
{
    public class BabbleDirt : MonoBehaviour
    {
        [SerializeField] private Bubble.Bubble bubble;
        [SerializeField] private Dirt.Dirt dirt;
        
        private Rigidbody rigid;
        private Vector3 pos;

        // Start is called before the first frame update
        void Start()
        {
            rigid = dirt.gameObject.GetComponent<Rigidbody>();
            pos = rigid.gameObject.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            rigid.useGravity = bubble.emit;

            if (!bubble.emit)
            {
                rigid.gameObject.transform.localPosition = pos;
            }
        }
    }
}
