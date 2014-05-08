﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept
{
    public static class FloatMath
    {
        public static float Ceiling(float n)
        {
            if (n < 0 || n == ((int)n))
                return (int)n;
            return ((int)n + 1);
        }

        public static float Floor(float n)
        {
            if (n > 0 || n == ((int)n))
                return (int)n;
            return ((int)n - 1);
        }

        public static float Truncate(float n)
        {
            return (int)n;
        }

        public static float Round(float n)
        {
           return (int) n + Math.Sign(n)*0.5f;
        }
    }
}
