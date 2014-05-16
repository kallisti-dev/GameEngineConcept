using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Graphics.VertexAttributes
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class VertexAttributeDefaults : Attribute
    {
        public bool DefaultNormalization { get; private set; }

        public VertexAttributeDefaults() : this(true) { }

        public VertexAttributeDefaults(bool defaultNormalization)
        {
            DefaultNormalization = defaultNormalization;
        }
    }
}
