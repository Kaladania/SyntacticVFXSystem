using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace SnSECS
{

    struct SnSDataComparer : IComparer<ComponentType>
    {
        public int Compare(ComponentType a, ComponentType b)
        {
            Elements aElement = TypeToElement(a);
            Elements bElement = TypeToElement(b);

            return bElement.CompareTo(aElement);
        }

        private Elements TypeToElement(ComponentType type)
        {
            if (type.TypeIndex == TypeManager.GetTypeIndex<SNSFireComponent>())
            {
                return Elements.FIRE;
            }
            else if (type.TypeIndex == TypeManager.GetTypeIndex<SNSWaterComponet>())
            {
                return Elements.WATER;
            }
            else if (type.TypeIndex == TypeManager.GetTypeIndex<SNSEarthComponet>())
            {
                return Elements.EARTH;
            }
            else if (type.TypeIndex == TypeManager.GetTypeIndex<SNSLightningComponet>())
            {
                return Elements.LIGHTNING;
            }

            return Elements.NONE;
        }
    }
}