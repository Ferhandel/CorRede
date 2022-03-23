
using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public static NetworkList<Color> colors = new NetworkList<Color>();

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

        }

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
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

        //Colores
        

        void SubmitColorRequestServerRpc(ServerRpcParams repcParams = default){
            
        }

       



        void Update()
        {
            transform.position = Position.Value;
        }
    }
}