using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// namespace Unicorn.Utilities
// {
    
    /// <summary>
    /// Various helper tools
    /// </summary>
    public static class UnicornExtensions
    {
        public static T GetRandomAndRemove<T>(this List<T> deck, bool retainOrder = false)
        {
            int index = Random.Range(0, deck.Count);
            T random = deck[index];
            if (retainOrder)
            {
                deck.RemoveAt(index);
            }
            else
            {
                deck[index] = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);
            }

            return random;
        }
        
        public static T GetRandom<T>(this List<T> deck)
        {
            int index = Random.Range(0, deck.Count);
            T random = deck[index];
            return random;
        }
        
        public static T GetRandom<T>(this T[] deck)
        {
            return deck[Random.Range(0, deck.Length)];
        }
        
        public static void Shuffle<T>(this List<T> deck)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                T temp = deck[i];
                int randomIndex = Random.Range(0, deck.Count);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }
        
        public static void Shuffle<T>(this T[] deck)
        {
            for (int i = 0; i < deck.Length; i++)
            {
                T temp = deck[i];
                int randomIndex = Random.Range(0, deck.Length);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Get a random point inside the provided bounds
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public static Vector3 RandomPoint(this Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }

        public static bool IsPointerOverUIObject()
        {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            //check touch
            if (Input.touchCount > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
            }

            return false;
        }

        #region Vector3

        /// <summary>
        /// </summary>
        /// <param name="angle"></param>
        /// <returns>Vector3 tạo với <c>Vector3.forward</c> một góc = angle</returns>
        public static Vector3 GetVectorFromAngle(float angle)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>Góc được tạo bởi <c>dir</c> và <c>Vector3.forward</c></returns>
        public static float GetAngleFromVector(Vector3 dir)
        {
            dir = dir.normalized;
            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            //int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static Vector3 Set(this Vector3 vector3, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x: x == null ? vector3.x : (float) x,
                y: y == null ? vector3.y : (float) y,
                z: z == null ? vector3.z : (float) z);
        }

        /// <summary>
        /// Move the vector towards the provided direction
        /// </summary>
        /// <param name="vector3"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 Move(this Vector3 vector3, Vector3 direction)
        {
            return vector3 + direction;
        }


        public static Vector2 Set(this Vector2 vector2, float? x = null, float? y = null)
        {
            return new Vector2(x: x == null ? vector2.x : (float) x,
                y: y == null ? vector2.y : (float) y);
        }

        public static Vector2 ToVectorXZ(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        public static Vector3 ToVectorXZ(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }

        public static bool Approximately(this Quaternion quatA, Quaternion value, float acceptableRange = 0.0004f)
        {
            return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
        }

        #endregion
    }
// }
