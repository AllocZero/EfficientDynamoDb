/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Amazon.Util.Internal
{
    public static partial class TypeFactory
    {
        class TypeInfoWrapper : AbstractTypeInfo
        {
            TypeInfo _typeInfo;

            internal TypeInfoWrapper(Type type)
                : base(type)
            {
                this._typeInfo = type.GetTypeInfo();
            }

            public override Type BaseType
            {
                get { return _typeInfo.BaseType; }
            }

            public override Type GetInterface(string name)
            {
                return this._typeInfo.ImplementedInterfaces.FirstOrDefault(x => (x.Namespace + "." + x.Name) == name);
            }

            public override Type[] GetInterfaces()
            {
                return this._typeInfo.ImplementedInterfaces.ToArray();
            }

            public override IEnumerable<PropertyInfo> GetProperties()
            {
                return this._type.GetProperties();
            }

            public override IEnumerable<FieldInfo> GetFields()
            {
                return this._type.GetFields();
            }

            public override FieldInfo GetField(string name)
            {
                return this._type.GetField(name);
            }

            public override MemberInfo[] GetMembers()
            {
                var members = GetMembers_Helper(this._typeInfo).Distinct().ToArray();
                return members;
            }
            private static readonly Type objectType = typeof(object);
            private static bool IsBackingField(MemberInfo mi)
            {
                var isBackingField = mi.Name.IndexOf("k__BackingField", StringComparison.Ordinal) >= 0;
                return isBackingField;
            }
            private static IEnumerable<MemberInfo> GetMembers_Helper(TypeInfo ti)
            {
                // Keep track of properties already returned. This makes sure properties that are overridden in sub classes are not returned back multiple times.
                var processedProperties = new HashSet<string>();
                Func<MemberInfo, bool> alreadyProcessProperty = (member) =>
                {
                    return (member is PropertyInfo) && !processedProperties.Add(member.Name);
                };

                var members = ti.DeclaredMembers;
                foreach (var member in members)
                {
                    if (!IsBackingField(member) && !alreadyProcessProperty(member))
                        yield return member;
                }

                var baseType = ti.BaseType;
                var isObject = (baseType == objectType);
                if (baseType != null && !isObject)
                {
                    var baseTi = baseType.GetTypeInfo();
                    var baseMembers = GetMembers_Helper(baseTi).ToList();

                    foreach (var baseMember in baseMembers)
                    {
                        if(!alreadyProcessProperty(baseMember))
                        {
                            yield return baseMember;
                        }
                    }
                }
            }

            public override bool IsClass
            {
                get { return this._typeInfo.IsClass; }
            }

            public override bool IsValueType
            {
                get { return this._typeInfo.IsValueType; }
            }

            public override bool IsInterface
            {
                get { return this._typeInfo.IsInterface; }
            }

            public override bool IsAbstract
            {
                get { return this._typeInfo.IsAbstract; }
            }

            public override bool IsSealed
            {
                get { return this._typeInfo.IsSealed; }
            }

            public override bool IsEnum
            {
                get { return this._typeInfo.IsEnum; }
            }

            public override MethodInfo GetMethod(string name)
            {
                return this._type.GetMethod(name);
            }

            public override bool ContainsGenericParameters
            {
                get { return this._typeInfo.ContainsGenericParameters; }
            }

            public override bool IsGenericTypeDefinition
            {
                get { return this._typeInfo.IsGenericTypeDefinition; }
            }

            public override bool IsGenericType
            {
                get
                {
                    return this._typeInfo.IsGenericType;
                }
            }

            public override Type GetGenericTypeDefinition()
            {
                return this._typeInfo.GetGenericTypeDefinition();
            }

            public override Type[] GetGenericArguments()
            {
                return this._typeInfo.GenericTypeArguments;
            }

            public override MethodInfo GetMethod(string name, ITypeInfo[] paramTypes)
            {
                Type[] types = new Type[paramTypes.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                    types[i] = ((AbstractTypeInfo)paramTypes[i]).Type;

                return this._type.GetMethod(name, types);
            }

            public override PropertyInfo GetProperty(string name)
            {
                return this._type.GetProperty(name);
            }

            public override bool IsAssignableFrom(ITypeInfo typeInfo)
            {
                return this._typeInfo.IsAssignableFrom(((TypeInfoWrapper)typeInfo)._typeInfo);
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                return CustomAttributeExtensions.GetCustomAttributes(this.Type.GetTypeInfo(), inherit).ToArray<object>();
            }

            public override object[] GetCustomAttributes(ITypeInfo attributeType, bool inherit)
            {
                return CustomAttributeExtensions.GetCustomAttributes(this.Type.GetTypeInfo(), attributeType.Type, inherit).ToArray<object>();
            }

            public override Assembly Assembly
            {
                get { return this._typeInfo.Assembly; }
            }

            public override ConstructorInfo GetConstructor(ITypeInfo[] paramTypes)
            {
                Type[] types = new Type[paramTypes.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                    types[i] = ((AbstractTypeInfo)paramTypes[i]).Type;

                return this._type.GetConstructor(types);
            }
        }
    }
}
