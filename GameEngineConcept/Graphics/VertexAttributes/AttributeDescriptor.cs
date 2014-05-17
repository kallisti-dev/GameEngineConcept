using System;


namespace GameEngineConcept.Graphics.VertexAttributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited=true)]
    public abstract class AttributeDescriptor : Attribute
    {
        public abstract void AddFields(VertexAttribute a);
    }
}
