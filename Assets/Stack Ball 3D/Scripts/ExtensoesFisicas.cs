using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCustomPhysicsExtensions
{
    public static class RigidbodyExtensions
    {
        public static void AddForceAltPosition(this Rigidbody rb, Vector3 force, Vector3 position, ForceMode mode)
        {
            // Exemplo de implementação: Aplica a força em uma posição alternativa usando AddForceAtPosition
            rb.AddForceAtPosition(force, position, mode);
        }
    }
}

