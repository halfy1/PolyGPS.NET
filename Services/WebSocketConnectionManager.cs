using System.Net.WebSockets;
using System.Collections.Concurrent;

public class WebSocketConnectionManager
{
    private ConcurrentDictionary<string, WebSocket> _sockets = new();

    public string AddSocket(WebSocket socket)
    {
        string connId = Guid.NewGuid().ToString();
        _sockets.TryAdd(connId, socket);
        return connId;
    }

    public ConcurrentDictionary<string, WebSocket> GetAllSockets()
    {
        return _sockets;
    }

    public void RemoveSocket(string id)
    {
        _sockets.TryRemove(id, out _);
    }
}