using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public static NetworkList<Color> colors = new NetworkList<Color>();

        int colorCount;

        private void Start() {
        

            colors.Add(Color.black);
            colors.Add(Color.blue);
            colors.Add(Color.cyan);
            colors.Add(Color.gray);
            colors.Add(Color.green);
            colors.Add(Color.white);
            colors.Add(Color.yellow);
            colors.Add(Color.magenta);
            colors.Add(Color.red);
            colors.Add(Color.grey);

            //StartCoroutine(ColorDrop());

        }

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
                //ColorDrop();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
                
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }


        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        [ServerRpc]
        
       /* IEnumerator ColorDrop(ServerRpcParams rpcParams = default){
            while(colorCount < 10){
                int selectedColors = Random.Range(0, colors.Count);
                GetComponent<MeshRenderer>(). material. color = colors[selectedColors];
                colors.RemoveAt(selectedColors);
                yield return new Color(Random.Range(0, colors.Count), colors.Count, Random.Range(0, colors.Count));
                colorCount += 1; 
            }
        }
        */

        void Update()
        {
            transform.position = Position.Value;
        }
        
    }
}