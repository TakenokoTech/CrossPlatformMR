using UnityEngine;

namespace Project.Scripts.Runtime.chat
{
    public class SyncObjectScript : MonoBehaviour
    {
        private void Start () {
        }
        
        private void Update () {
            if (Input.GetKey("up")) {
                transform.position += transform.forward * 0.05f;
            }
            if (Input.GetKey("right")) {
                transform.Rotate(0, 10, 0);
            }
            if (Input.GetKey ("left")) {
                transform.Rotate(0, -10, 0);
            }
        }
    }
}