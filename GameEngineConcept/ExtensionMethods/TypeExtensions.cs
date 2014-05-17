using OpenTK.Graphics.OpenGL4;
using System;

namespace GameEngineConcept.ExtensionMethods
{
    using Util;

    public static class TypeExtensions
    {

        //convert a C# type into its corresponding VertexAttribPointerType
        public static VertexAttribPointerType? GetVertexAttribPointerType(this Type t)
        {
            if (t == typeof(float))
                return VertexAttribPointerType.Float;
            if (t == typeof(double))
                return VertexAttribPointerType.Double;
            if (t == typeof(byte))
                return VertexAttribPointerType.UnsignedByte;
            if (t == typeof(sbyte))
                return VertexAttribPointerType.Byte;
            if (t == typeof(int))
                return VertexAttribPointerType.Int;
            if (t == typeof(uint))
                return VertexAttribPointerType.UnsignedInt;
            if (t == typeof(short))
                return VertexAttribPointerType.Short;
            if (t == typeof(ushort))
                return VertexAttribPointerType.UnsignedShort;
            if (t == typeof(char))
                return VertexAttribPointerType.UnsignedShort;
            return null;
        }

        public static VertexAttribPointerType? GetComponentAttribPointerType(this Type t)
        {
            VertexAttribPointerType? aType = t.GetVertexAttribPointerType();
            if (aType.HasValue)
                return aType.Value;
            foreach (var field in t.GetFields(Util.AllInstanceFields)) {
                var fieldAType = field.FieldType.GetVertexAttribPointerType();
                if (aType == null) {
                    aType = fieldAType;
                }
                else if (aType != fieldAType) {
                    return null;
                }
            }
            return aType;
        }
    }
}
