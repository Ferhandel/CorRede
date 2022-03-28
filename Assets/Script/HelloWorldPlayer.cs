using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public  NetworkVariable<Color> colorPlayer = new NetworkVariable<Color>();
        public static List<Color> avariableColors = new List<Color>();

        Renderer ren;

        //int colorCount;

        private void Start() {
            Position.OnValueChanged += OnPositionChange;
            ren = GetComponent<Renderer>();
            
            if(IsServer && IsOwner){
            avariableColors.Add(Color.black);
            avariableColors.Add(Color.blue);
            avariableColors.Add(Color.cyan);
            avariableColors.Add(Color.gray);
            avariableColors.Add(Color.green);
            avariableColors.Add(Color.white);
            avariableColors.Add(Color.yellow);
            avariableColors.Add(Color.magenta);
            avariableColors.Add(Color.red);
            //checkeamos los avariableColors
            Debug.Log(avariableColors.Count);
            }
           
            //StartCoroutine(ColorDrop());
        }

        //Para no hacer que cargue en el update de manera innecesaria aunque la posicion sea la misma.
        public void OnPositionChange(Vector3 previusValue, Vector3 newValue){
            transform.position = Position.Value;
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
         public void SubmitColorRequestServerRpc(ServerRpcParams rpcParams = default){
            //usamos oldColor para no terminar sin colores en la lista. 
            Color oldColor = colorPlayer.Value;
            Color newColor = avariableColors[Random.Range(0, avariableColors.Count)];
            //quitamos el color por defecto del player (que siempre es negro)
            avariableColors.Remove(newColor);
            //añadimos ese color a la lista avariableColors
            avariableColors.Add(oldColor);
            //newColor se guarda dentro de colorPlayer ( que es networkvariable, por lo tanto se expande al resto de objetos)
            colorPlayer.Value = newColor;
            Debug.Log(pretty(avariableColors));
        }

        private string pretty(List<Color>l){
            string result = "";
            foreach (Color item in l){
                result += item.ToString() + " ";
            }
            return result;
        }

        void Update()
        {
            ren.material.SetColor("_Color", colorPlayer.Value);
            
        }
        
    }
}