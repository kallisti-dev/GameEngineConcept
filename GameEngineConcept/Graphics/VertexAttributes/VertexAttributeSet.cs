using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameEngineConcept.Graphics.VertexAttributes
{

    using ExtensionMethods;
    using VertexAttributes;

    public class VertexAttributeSet : SortedSet<VertexAttribute>
    {
        public static Comparer<VertexAttribute> cmp = Util.CompareBy<VertexAttribute, int>((attr) => attr.index);

        private static Dictionary<Type, VertexAttributeSet> attrCache = new Dictionary<Type, VertexAttributeSet>();

        public static VertexAttributeSet FromType<T>() where T : struct
        {
            return FromType(typeof(T));
        }

        public static VertexAttributeSet FromType(Type t)
        {
            VertexAttributeSet @out;
            if (!attrCache.TryGetValue(t, out @out)) {
                attrCache[t] = @out = Create(t);
            }
            return @out;
        }



        private static VertexAttributeSet Create(Type t)
        {
            if (t.IsPrimitive || !t.IsValueType)
                throw new ArgumentException(t + " is not a non-primitive value type.");

            var vs = t.GetCustomAttribute<VertexStruct>() ?? new VertexStruct();

            return new VertexAttributeSet(
                t.GetCustomAttributes<VertexAttribute>()
                .Concat(
                    t.GetFields(Util.AllInstanceFields)
                    .Select((field, i) => {
                        var attr = new VertexAttribute { //initialize default vertex attribute for current field
                            index = i,
                            nComponents = GetNComponents(field.FieldType),
                            normalized = vs.DefaultNormalization,
                            stride = Marshal.SizeOf(t),
                            offset = (int)Marshal.OffsetOf(t, field.Name)
                        };

                        bool foundComponentType = false;  //apply all AttributeDescriptors on the current field 
                        foreach (var descriptor in field.GetCustomAttributes<AttributeDescriptor>()) {
                            descriptor.AddFields(attr);
                            if (descriptor is ComponentType)
                                foundComponentType = true;
                        }

                        if (!foundComponentType) {  //if attr.type was never initialized...
                            var maybeType = field.FieldType.GetComponentAttribPointerType(); //attempt to determine component types
                            if (maybeType.HasValue)
                                attr.type = maybeType.Value;
                            else
                                throw new ArgumentException("Cannot determine component type for field named " + field.Name);
                        }
                        return attr;
                    })
                )
            );
        }

        private static int GetNComponents(Type t)
        {
            if (!t.IsValueType)
                throw new ArgumentException(t + " is not a value type.");
            if (t.IsPrimitive)
                return 1;
            int n = 0;
            foreach (var field in t.GetFields(Util.AllInstanceFields)) {
                n += GetNComponents(field.FieldType);
            }
            return n;
        }

        public ISet<int> Indices
        {
            get { return new HashSet<int>(this.Select((attr) => attr.index)); }
        }

        public VertexAttributeSet() : base(cmp) { }
        public VertexAttributeSet(IEnumerable<VertexAttribute> e) : base(e, cmp) { }

    }
}
