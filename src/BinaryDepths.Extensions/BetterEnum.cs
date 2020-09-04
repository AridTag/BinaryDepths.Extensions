using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace BinaryDepths.Extensions
{
    /// <summary>
    /// <c>BetterEnum</c> is a class that simulates the functionality available to a <c>Java</c> enum 
    /// in that each value of the enum is really just a "static" readonly class "instance" with a comparable value and all that being a class affords
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TDerived"></typeparam>
    /// <example>
    /// <para>The following examples use the class defined at the bottom of this example </para>
    /// <para>
    /// Using this class you can essentially extend the functionality of an enum value and perform operations with it
    /// while retaining all functionality of enum except where an enum value could be used in a static context like a case statement (pre C# 7.3)
    /// </para>
    /// <code>var someResult = SomeEnum.Value0.DoSomething("Hello");</code>
    /// <para>
    /// Now (post C# 7.3) we can use pattern matching for a case statement. It's not ideal as it requires a bit more code to write but it can be done
    /// </para>
    /// <code>
    /// SomeEnum e = GetSomeValue();
    /// switch (e)
    /// {
    ///     case var val0 when val0 == Value0:
    ///         break;
    ///                 
    ///     case var val1 when val1 == Value1:
    ///         break;
    /// }
    /// </code>
    /// <para>This is the definition of the enum class used</para>
    /// <code>
    /// public class SomeEnum : BetterEnum&lt;int, SomeEnum&lt;
    /// {
    ///     public static readonly SomeEnum Value0 = new SomeEnum(0);
    ///     public static readonly SomeEnum Value1 = new SomeEnum(1);
    ///     public static readonly SomeEnum Value2 = new SomeEnum(2);
    ///     
    ///     private SomeEnum(int value)
    ///             : base(value)
    ///     {
    ///     }
    /// 
    ///     public int DoSomething(object param)
    ///     {
    ///         return Value + param.GetHashCode();
    ///     }
    /// }
    /// </code>
    /// </example>
    [DebuggerDisplay("{Value} ({Name})")]
    public abstract class BetterEnum<TValue, TDerived> : IEquatable<TDerived>, IComparable<TDerived>, IComparable, IComparer<TDerived>
                                                         where TValue : struct, IComparable<TValue>, IEquatable<TValue>
                                                         where TDerived : BetterEnum<TValue, TDerived>
    {
        private static bool _IsInitialized;
        
        /// <summary>
        /// Reverse lookup to convert values back to local instances
        /// </summary>
        private static SortedList<TValue, TDerived> _Values;
        
        /// <summary>
        /// The value of the enum item
        /// </summary>
        public readonly TValue Value;

        /// <summary>
        /// The DescriptionAttribute, if any, linked to the declaring field
        /// </summary>
        private DescriptionAttribute _DescriptionAttribute;

        private string _Name;
        private string _DisplayName;
        
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing all the values defined in this enum
        /// </summary>
        public static IEnumerable<TDerived> Values => _Values.Values;

        /// <summary>
        /// Attempts to convert the given string into a value represented by this enum
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The value or <c>default</c> if not found</returns>
        public static TDerived Parse(string name)
        {
            foreach (var value in _Values.Values)
            {
                if (string.Compare(value.Name, name, true) == 0 || string.Compare(value.DisplayName, name, true) == 0)
                    return value;
            }

            return default;
        }
        
        /// <summary>
        /// The name of the enum item
        /// </summary>
        public string Name
        {
            get
            {
                EnsureReflected();
                return _Name;
            }
        }
        
        /// <summary>
        /// The display name of the enum item
        /// </summary>
        public string DisplayName
        {
            get
            {
                EnsureReflected();
                return _DisplayName;
            }
        }

        /// <summary>
        /// The description of the enum item as defined by the linked <see cref="DescriptionAttribute"/> if any
        /// or <see cref="Name"/> if no attribute
        /// </summary>
        public string Description
        {
            get
            {
                EnsureReflected();
                return _DescriptionAttribute?.Description ?? _Name;
            }
        }

        protected BetterEnum(TValue value)
        {
            if (_Values == null)
                _Values = new SortedList<TValue, TDerived>();
            
            Value = value;
            _Values.Add(value, (TDerived)this);
        }

        private static void EnsureReflected()
        {
            if (_IsInitialized)
                return;

            foreach (var (value, fieldInfo) in GetFields())
            {
                value._Name = fieldInfo.Name;
                value._DescriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                value._DisplayName = fieldInfo.Name.SplitCamelCase();
            }
            
            _IsInitialized = true;
        }

        private static IEnumerable<(TDerived value, FieldInfo fieldInfo)> GetFields()
        {
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public;
            return typeof(TDerived).GetFields(bindingFlags)
                                   .Where(t => IsTypeTDerived(t.FieldType))
                                   .Select(f => ((TDerived)f.GetValue(null), f));
        }

        private static bool IsTypeTDerived(Type t)
        {
            return t == typeof(TDerived);
        }

        #region Conversion and Equality

        public static TDerived Convert(TValue value)
        {
            return _Values[value];
        }

        public static bool TryConvert(TValue value, out TDerived result)
        {
            return _Values.TryGetValue(value, out result);
        }

        public static implicit operator TValue(BetterEnum<TValue, TDerived> value)
        {
            return value.Value;
        }

        public static implicit operator BetterEnum<TValue, TDerived>(TValue value)
        {
            return _Values[value];
        }

        public static implicit operator TDerived(BetterEnum<TValue, TDerived> value)
        {
            return value;
        }

        public override string ToString()
        {
            return _Name;
        }

        #endregion

        #region IEquatable<TDerived> Members

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                if (obj is TValue tvalue)
                    return Value.Equals(tvalue);

                if (obj is TDerived tderived)
                    return Value.Equals(tderived.Value);
            }

            return false;
        }

        bool IEquatable<TDerived>.Equals(TDerived other)
        {
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        #endregion

        #region IComparable Members
        int IComparable<TDerived>.CompareTo(TDerived other)
        {
            return Value.CompareTo(other.Value);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj != null)
            {
                if (obj is TValue)
                    return Value.CompareTo((TValue)obj);

                if (obj is TDerived)
                    return Value.CompareTo(((TDerived)obj).Value);
            }
            return -1;
        }

        int IComparer<TDerived>.Compare(TDerived x, TDerived y)
        {
            return (x == null) ? -1 :
                   (y == null) ? 1 :
                    x.Value.CompareTo(y.Value);
        }

        #endregion
    }
}
