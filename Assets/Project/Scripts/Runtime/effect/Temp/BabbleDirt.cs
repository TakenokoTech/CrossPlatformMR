using UnityEngine;

namespace Project.Scripts.Runtime.effect.Temp
{
    public class BabbleDirt : MonoBehaviour
    {
        [SerializeField] private Bubble.Bubble bubble;
        [SerializeField] private Dirt.Dirt dirt;
        [SerializeField] internal bool emit = false;
        
        private Rigidbody rigid;
        private Vector3 pos;
        private Quaternion rot;

        // Start is called before the first frame update
        void Start()
        {
            rigid = dirt.gameObject.GetComponent<Rigidbody>();
            pos = rigid.gameObject.transform.localPosition;
            rot = rigid.gameObject.transform.localRotation;
        }

        // Update is called once per frame
        void Update()
        {
            rigid.useGravity = bubble.emit;
            bubble.emit = emit;

            if (!bubble.emit)
            {
                rigid.gameObject.transform.localPosition = pos;
                rigid.gameObject.transform.localRotation = rot;
            }
        }
    }
}
