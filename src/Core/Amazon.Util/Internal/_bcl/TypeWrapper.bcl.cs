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

            internal TypeInfoWrapper(Type type)
                : base(type)
            {
            }

            public override Type BaseType
            {
                get { return this._type.BaseType; }
            }

            public override FieldInfo GetField(string name)
            {
                return this._type.GetField(name);
            }

            public override Type GetInterface(string name)
            {
                return this._type.GetInterfaces().FirstOrDefault(x => (x.Namespace + "." + x.Name) == name);
            }

            public override Type[] GetInterfaces()
            {
                return this._type.GetInterfaces();
            }

            public override IEnumerable<PropertyInfo> GetProperties()
            {
                return this._type.GetProperties();
            }

            public override IEnumerable<FieldInfo> GetFields()
            {
                return this._type.GetFields();
            }

            public override MemberInfo[] GetMembers()
            {
                return this._type.GetMembers();
            }

            public override bool IsClass
            {
                get { return this._type.IsClass; }
            }

            public override bool IsValueType
            {
                get { return this._type.IsValueType; }
            }

            public override bool IsInterface
            {
                get { return this._type.IsInterface; }
            }

            public override bool IsAbstract
            {
                get { return this._type.IsAbstract; }
            }

            public override bool IsSealed
            {
                get { return this._type.IsSealed; }
            }

            public override bool IsEnum
            {
                get { return this._type.IsEnum; }
            }

            public override MethodInfo GetMethod(string name)
            {
                return this._type.GetMethod(name);
            }

            public override MethodInfo GetMethod(string name, ITypeInfo[] paramTypes)
            {
                Type[] types = new Type[paramTypes.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                    types[i] = ((AbstractTypeInfo)paramTypes[i]).Type;
                return this._type.GetMethod(name, types);
            }

            public override ConstructorInfo GetConstructor(ITypeInfo[] paramTypes)
            {
                Type[] types = new Type[paramTypes.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                    types[i] = ((AbstractTypeInfo)paramTypes[i]).Type;
                var constructor = this._type.GetConstructor(types);
                return constructor;
            }

            public override PropertyInfo GetProperty(string name)
            {
                return this._type.GetProperty(name);
            }

            public override bool IsAssignableFrom(ITypeInfo typeInfo)
            {
                return this._type.IsAssignableFrom(((AbstractTypeInfo)typeInfo).Type);
            }

            public override bool ContainsGenericParameters
            {
                get { return this._type.ContainsGenericParameters; }
            }

            public override bool IsGenericTypeDefinition
            {
                get { return this._type.IsGenericTypeDefinition; }
            }

            public override bool IsGenericType
            {
                get { return this._type.IsGenericType; }
            }

            public override Type GetGenericTypeDefinition()
            {
                return this._type.GetGenericTypeDefinition();
            }

            public override Type[] GetGenericArguments()
            {
                return this._type.GetGenericArguments();
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                return this._type.GetCustomAttributes(inherit);
            }

            public override object[] GetCustomAttributes(ITypeInfo attributeType, bool inherit)
            {
                return this._type.GetCustomAttributes(((TypeInfoWrapper)attributeType)._type, inherit);
            }

            public override Assembly Assembly
            {
                get { return this._type.Assembly; }
            }
        }
    
    }
}
