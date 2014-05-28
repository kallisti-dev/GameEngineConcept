using MiscUtil;
using System;

namespace GameEngineConcept.Components.Properties
{

    public static class Property
    {
        /* Convenience functions for creating properties whose values derive from other properties,
         * and whose values are recomputed when their parent properties change
         */

        //Depends on one parent
        public static Property<B> Create<A, B>(Property<A> property, Func<A, B> f)
        {
            var newProp = new Property<B>(f(property));
            property.OnValueChanged += (p, newVal) => { newProp.Value = f(newVal); };
            return newProp;
        }

        //Depends on two parents
        public static Property<C> Create<A, B, C>(Property<A> propertyA, Property<B> propertyB, Func<A, B, C> f)
        {
            var newProp = new Property<C>(f(propertyA, propertyB));
            propertyA.OnValueChanged += (p, newVal) => { newProp.Value = f(newVal, propertyB); };
            propertyB.OnValueChanged += (p, newVal) => { newProp.Value = f(propertyA, newVal); };
            return newProp;
        }

        //Depends on three parents
        public static Property<D> Create<A, B, C, D>(Property<A> propertyA, Property<B> propertyB, Property<C> propertyC, Func<A, B, C, D> f)
        {
            var newProp = new Property<D>(f(propertyA, propertyB, propertyC));
            propertyA.OnValueChanged += (p, newA) => { newProp.Value = f(newA, propertyB, propertyC); };
            propertyB.OnValueChanged += (p, newB) => { newProp.Value = f(propertyA, newB, propertyC); };
            propertyC.OnValueChanged += (p, newC) => { newProp.Value = f(propertyA, propertyB, newC); };
            return newProp;
        }

        //Creates a property whose value varies on a condition property
        public static Property<A> If<A>(Property<bool> condition, Func<A> trueCase, Func<A> falseCase)
        {
            return If(condition, (b) => b, trueCase, falseCase);
        }

        public static Property<B> If<A, B>(Property<A> testProp, Func<A, bool> testFunc, Func<B> trueCase, Func<B> falseCase)
        {
            return Create(testProp, (val) =>
                testFunc(val) ? trueCase() : falseCase()
            );
        }
    }

    public class Property<T>
    {

        /* An event that is triggered when the value of the property is reset
         * first parameter is the property itself, with the previous value in-tact
         * second parameter is the new value being set as the property's value
         */
        public event Action<Property<T>, T> OnValueChanged = (p, t) => { };

        /* The inner value of the property */
        protected T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                OnValueChanged(this, value);
                _value = value;
            }
        }

        /* Constructor */
        public Property(T value)
        {
            _value = value;
        }

        /* Properties can be implicitly converted to their inner type */
        public static implicit operator T(Property<T> prop)
        {
            return prop.Value;
        }

        /* Derives a new property from this property.
         * 
         * When the value of this property changes, the derived property will be 
         * automatically recomputed using the supplied function.
         */
        public Property<A> Derive<A>(Func<T, A> f)
        {
            return Property.Create(this, f);
        }

        /* Same as the static Property.If method */
        public Property<A> If<A>(Func<T, bool> testFunc, Func<A> trueCase, Func<A> falseCase)
        {
            return Property.If(this, testFunc, trueCase, falseCase);
        }

        /* Addition of properties */
        public static Property<T> operator +(Property<T> a, Property<T> b)
        {
            return Property.Create(a, b, Operator.Add);
        }


        /* Multiplication of properties */
        public static Property<T> operator *(Property<T> a, Property<T> b)
        {
            return Property.Create(a, b, Operator.Multiply);
        }

        /* Division of numeric properties */ 
        public static Property<T> operator /(Property<T> a, Property<T> b)
        {
            return Property.Create(a, b, Operator.Divide);
        }
    }
}
