using Unity.Entities;
using Unity.Mathematics;

namespace SnSECS
{
    /// <summary>
    /// Holds position information
    /// </summary>
    public class SNSTransformComponent
    {
        public float3 _position;
        public float3 _rotation;
        public float3 _scale;
    }

}
