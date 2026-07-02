using UnityEngine;

/// <summary>
/// Classe abstrata base para componentes que se subscrevem a eventos do PlayerInputReader.
/// Gerencia automaticamente a conexão/desconexão de eventos durante OnEnable/OnDisable.
/// </summary>
public abstract class PlayerInputSubscriber : MonoBehaviour
{
    protected PlayerInputReader playerInputReader;
    private bool isConnected;

    /// <summary>
    /// Inicializa o subscriber com o leitor de input.
    /// Conecta automaticamente se o GameObject está ativo.
    /// </summary>
    public virtual void Initialize(PlayerInputReader reader)
    {
        playerInputReader = reader;

        if (isActiveAndEnabled)
        {
            Connect();
        }
    }

    protected virtual void OnEnable()
    {
        Connect();
    }

    protected virtual void OnDisable()
    {
        Disconnect();
    }

    /// <summary>
    /// Conecta os eventos do input reader. Implementado pelas subclasses.
    /// </summary>
    protected virtual void Connect()
    {
        if (isConnected)
            return;

        if (playerInputReader == null)
        {
            Debug.LogWarning($"{GetType().Name}: PlayerInputReader is null. Cannot connect to input events.", this);
            return;
        }

        ConnectToInputEvents();
        isConnected = true;
    }

    /// <summary>
    /// Desconecta os eventos do input reader. Implementado pelas subclasses.
    /// </summary>
    protected virtual void Disconnect()
    {
        if (!isConnected)
            return;

        DisconnectFromInputEvents();
        isConnected = false;
    }

    /// <summary>
    /// Conecta os eventos específicos do input reader.
    /// Deve ser implementado pelas subclasses.
    /// </summary>
    protected abstract void ConnectToInputEvents();

    /// <summary>
    /// Desconecta os eventos específicos do input reader.
    /// Deve ser implementado pelas subclasses.
    /// </summary>
    protected abstract void DisconnectFromInputEvents();
}
