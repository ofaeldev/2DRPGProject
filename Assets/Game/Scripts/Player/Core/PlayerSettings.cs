using UnityEngine;

/// <summary>
/// Scriptable Object para armazenar configurações globais do jogador.
/// Permite editar valores no Inspector e reutilizar em diferentes prefabs/instâncias.
/// </summary>
[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Game/Player Settings", order = 0)]
public class PlayerSettings : ScriptableObject
{
    [Header("Movimento")]
    [Tooltip("Velocidade de movimento do jogador (units/segundo)")]
    [Range(1f, 10f)]
    public float moveSpeed = 5f;

    [Header("Interação")]
    [Tooltip("Distância máxima de interação (units)")]
    [Range(0.5f, 3f)]
    public float interactionDistance = 1.5f;

    [Tooltip("Raio de detecção para interação (units)")]
    [Range(0.1f, 2f)]
    public float interactionRadius = 0.5f;

    [Header("Inventário")]
    [Tooltip("Número máximo de slots no inventário")]
    [Range(6, 30)]
    public int maxInventorySlots = 12;
}
