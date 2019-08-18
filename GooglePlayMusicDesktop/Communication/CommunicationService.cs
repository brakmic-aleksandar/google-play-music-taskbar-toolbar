using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GooglePlayMusicDesktop.Communication.DataTransferObject;
using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication
{
    internal class CommunicationService
    {
        private ClientWebSocket _clientWebSocket;
        private Thread _listeningThread; 
        private int _nextRequestId;
        private Dictionary<long, EventWaitHandle> currentlyAwaitingResponseEvents = new Dictionary<long, EventWaitHandle>();
        private Dictionary<long, ControllerResponse> currentlyAwaitingResponses = new Dictionary<long, ControllerResponse>();
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public bool IsConnected { get; private set; }
        public event EventHandler<MessageEventArgs> MessageRecieved;
        public event EventHandler<EventArgs> OnConnected;

        public CommunicationService()
        {
            Connect();
        }

        public async Task<ControllerResponse> SendRequestAsync(ControllerRequest request)
        {
            return await Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                try
                {
                    request.Id = _nextRequestId++;
                    byte[] data = GetBytes(request);
                    var buffer = new ArraySegment<byte>(data);
                    var resetEvent = new AutoResetEvent(false);
                    if (request.HasResponse)
                    {
                        currentlyAwaitingResponseEvents.Add(request.Id, resetEvent);
                    }
                    await _clientWebSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);

                    if (request.HasResponse)
                    {
                        resetEvent.WaitOne();
                        if (currentlyAwaitingResponses.TryGetValue(request.Id, out var latestResponse))
                        {
                            return latestResponse;
                        }
                        return null;
                    }
                    return null;
                }
                finally
                {
                    semaphore.Release();
                }
            });
        }

        private async void Connect()
        {
            try
            {
                _clientWebSocket = new ClientWebSocket();
                await _clientWebSocket.ConnectAsync(new Uri("ws://localhost:5672"), CancellationToken.None);
                IsConnected = true;
                StartListening();
                OnConnected?.Invoke(this, EventArgs.Empty);
            }
            catch (WebSocketException)
            {
                await Task.Delay(3000);
                Connect();
            }
        }

        private void StartListening()
        {
            _listeningThread = new Thread(Listen);
            _listeningThread.Start();
        }

        private async void Listen()
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            var bytes = new List<byte>();
            while (IsConnected)
            {
                WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                if (buffer.Array != null)
                {
                    bytes.AddRange(buffer.Array.Take(result.Count));
                }
                if (result.EndOfMessage)
                {
                    string json = GetJson(bytes);
                    if(json.StartsWith("{\"channel\""))
                    {
                        Message data = ResponseMessageFactory.Instance.Create(json);
                        DispatchMessageEvent(data);
                    }
                    else
                    {
                        var response = JsonConvert.DeserializeObject<ControllerResponse>(json);
                        if (currentlyAwaitingResponseEvents.TryGetValue(response.RequestId, out var e))
                        {
                            currentlyAwaitingResponses.Add(response.RequestId, response);
                            currentlyAwaitingResponseEvents.Remove(response.RequestId);
                            e.Set();
                        }
                    }
                    bytes.Clear();
                }
            }
        }

        private string GetJson(List<byte> bytes)
        {
            byte[] byteArr = bytes.ToArray();
            return Encoding.UTF8.GetString(byteArr, 0, byteArr.Length);
        }

        private string GetJson(ControllerRequest data)
        {
            return JsonConvert.SerializeObject(data);
        }

        private byte[] GetBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        private byte[] GetBytes(ControllerRequest data)
        {
            return GetBytes(GetJson(data));
        }

        private void DispatchMessageEvent(Message message)
        {
            MessageRecieved?.Invoke(this, new MessageEventArgs(message));
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public Message Message { get; }

        public MessageEventArgs(Message message)
        {
            Message = message;
        }
    }
}
