using _Scripts.Player;
using Unity.Netcode;
using UnityEngine;

namespace _Scripts
{
    public class HelloWorldManager : MonoBehaviour
    {
        private NetworkManager _mNetworkManager;

        void Awake()
        {
            _mNetworkManager = GetComponentInParent<NetworkManager>();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!_mNetworkManager.IsClient && !_mNetworkManager.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (GUILayout.Button("Host")) _mNetworkManager.StartHost();
            if (GUILayout.Button("Client")) _mNetworkManager.StartClient();
            if (GUILayout.Button("Server")) _mNetworkManager.StartServer();
        }

        void StatusLabels()
        {
            var mode = _mNetworkManager.IsHost ?
                "Host" : _mNetworkManager.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                _mNetworkManager.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
            if (GUILayout.Button("Stop Connection")) _mNetworkManager.Shutdown();
        }

        void SubmitNewPosition()
        {
            if (GUILayout.Button(_mNetworkManager.IsServer ? "Move" : "Request Position Change"))
            {
                if (_mNetworkManager.IsServer && !_mNetworkManager.IsClient )
                {
                    foreach (ulong uid in _mNetworkManager.ConnectedClientsIds)
                        _mNetworkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<SamplePlayer>().Move();
                }
                else
                {
                    NetworkObject playerObject = _mNetworkManager.SpawnManager.GetLocalPlayerObject();
                    SamplePlayer player = playerObject.GetComponent<SamplePlayer>();
                    player.Move();
                }
            }
        }
    }
}