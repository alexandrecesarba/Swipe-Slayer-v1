// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {

//     public float moveSpeed = 15f;
//     public float tileSize = 1f;
//     public Transform movePoint; // pra onde o player vai mover

//     public LayerMask whatStopsMovement;
//     // Start is called before the first frame update
//     void Start()
//     {
//         movePoint.parent = null;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

//         if(Vector3.Distance(transform.position, movePoint.position) <= .02f) {

//             if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) {
//                 Debug.Log("H = " + Input.GetAxisRaw("Horizontal"));
//                 if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, whatStopsMovement))
//                     movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal")*tileSize, 0f, 0f);

//             }

//             if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) {
//                 if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopsMovement))
//                     movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical")*tileSize, 0f);
//                 Debug.Log("V = " + Input.GetAxisRaw("Vertical"));
//             }
//         }
//     }
// }
