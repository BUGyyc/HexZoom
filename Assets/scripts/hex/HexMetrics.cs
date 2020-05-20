/*
 * @Author: delevin.ying 
 * @Date: 2020-05-08 17:17:05 
 * @Last Modified by: delevin.ying
 * @Last Modified time: 2020-05-20 17:10:13
 */
using UnityEngine;
namespace Hex
{
    public static class HexMetrics
    {
        /// <summary>
        /// 外圈半径
        /// </summary>
        public const float outerRadius = 10f;
        /// <summary>
        /// 内圈半径
        /// </summary>
        public const float innerRadius = outerRadius * 0.866025404f;
        public const float innerDiameter = innerRadius * 2f;

        public const float solidFactor = 0.75f;
        public const float blendFactor = 1f - solidFactor;
        public static Vector3[] corners =
        {
            new Vector3(0f, 0f, outerRadius),
            new Vector3(innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(0f, 0f, -outerRadius),
            new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f, 0f, outerRadius),
        };

        public static Vector3 GetFirstCorner(HexDirection direction)
        {
            return corners[(int)direction] * solidFactor;
        }

        public static Vector3 GetSecondCorner(HexDirection direction)
        {
            return corners[(int)direction + 1] * solidFactor            ;
        }
    }
}