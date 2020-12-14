// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;

namespace EfficientDynamoDb.Internal.Mapping
{
    internal static class EmitMemberAccessor
    {
        public static readonly Type ObjectType = typeof(object);
        
        public static Func<object>? CreateConstructor(Type type)
        {
            Debug.Assert(type != null);
            ConstructorInfo? realMethod = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, binder: null, Type.EmptyTypes, modifiers: null);

            if (type.IsAbstract)
            {
                return null;
            }

            if (realMethod == null && !type.IsValueType)
            {
                return null;
            }

            var dynamicMethod = new DynamicMethod(
                ConstructorInfo.ConstructorName,
                ObjectType,
                Type.EmptyTypes,
                typeof(EmitMemberAccessor).Module,
                skipVisibility: true);

            ILGenerator generator = dynamicMethod.GetILGenerator();

            if (realMethod == null)
            {
                LocalBuilder local = generator.DeclareLocal(type);

                generator.Emit(OpCodes.Ldloca_S, local);
                generator.Emit(OpCodes.Initobj, type);
                generator.Emit(OpCodes.Ldloc, local);
                generator.Emit(OpCodes.Box, type);
            }
            else
            {
                generator.Emit(OpCodes.Newobj, realMethod);
            }

            generator.Emit(OpCodes.Ret);

            return (Func<object>?)dynamicMethod.CreateDelegate(typeof(Func<object>));
        }
        
        public static Func<object, TProperty> CreatePropertyGetter<TProperty>(PropertyInfo propertyInfo) =>
            CreateDelegate<Func<object, TProperty>>(CreatePropertyGetter(propertyInfo, typeof(TProperty)));
        
        private static DynamicMethod CreatePropertyGetter(PropertyInfo propertyInfo, Type runtimePropertyType)
        {
            MethodInfo? realMethod = propertyInfo.GetMethod;
            Debug.Assert(realMethod != null);

            Type? declaringType = propertyInfo.DeclaringType;
            Debug.Assert(declaringType != null);

            Type declaredPropertyType = propertyInfo.PropertyType;

            DynamicMethod dynamicMethod = CreateGetterMethod(propertyInfo.Name, runtimePropertyType);
            ILGenerator generator = dynamicMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);

            if (declaringType.IsValueType)
            {
                generator.Emit(OpCodes.Unbox, declaringType);
                generator.Emit(OpCodes.Call, realMethod);
            }
            else
            {
                generator.Emit(OpCodes.Castclass, declaringType);
                generator.Emit(OpCodes.Callvirt, realMethod);
            }

            // declaredPropertyType: Type of the property
            // runtimePropertyType:  <T> of JsonConverter / JsonPropertyInfo

            if (declaredPropertyType != runtimePropertyType && declaredPropertyType.IsValueType)
            {
                generator.Emit(OpCodes.Box, declaredPropertyType);
            }

            generator.Emit(OpCodes.Ret);

            return dynamicMethod;
        }

        public static Action<object, TProperty> CreatePropertySetter<TProperty>(PropertyInfo propertyInfo) =>
            CreateDelegate<Action<object, TProperty>>(CreatePropertySetter(propertyInfo, typeof(TProperty)));

        private static DynamicMethod CreatePropertySetter(PropertyInfo propertyInfo, Type runtimePropertyType)
        {
            MethodInfo? realMethod = propertyInfo.SetMethod;
            Debug.Assert(realMethod != null);

            Type? declaringType = propertyInfo.DeclaringType;
            Debug.Assert(declaringType != null);

            Type declaredPropertyType = propertyInfo.PropertyType;

            DynamicMethod dynamicMethod = CreateSetterMethod(propertyInfo.Name, runtimePropertyType);
            ILGenerator generator = dynamicMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(declaringType.IsValueType ? OpCodes.Unbox : OpCodes.Castclass, declaringType);
            generator.Emit(OpCodes.Ldarg_1);

            // declaredPropertyType: Type of the property
            // runtimePropertyType:  <T> of JsonConverter / JsonPropertyInfo

            if (declaredPropertyType != runtimePropertyType && declaredPropertyType.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, declaredPropertyType);
            }

            generator.Emit(declaringType.IsValueType ? OpCodes.Call : OpCodes.Callvirt, realMethod);
            generator.Emit(OpCodes.Ret);

            return dynamicMethod;
        }

        private static DynamicMethod CreateGetterMethod(string memberName, Type memberType) =>
            new DynamicMethod(
                memberName + "Getter",
                memberType,
                new[] {ObjectType},
                typeof(EmitMemberAccessor).Module,
                skipVisibility: true);

        private static DynamicMethod CreateSetterMethod(string memberName, Type memberType) =>
            new DynamicMethod(
                memberName + "Setter",
                typeof(void),
                new[] {ObjectType, memberType},
                typeof(EmitMemberAccessor).Module,
                skipVisibility: true);

        [return: NotNullIfNotNull("method")]
        private static T? CreateDelegate<T>(DynamicMethod? method) where T : Delegate =>
            (T?) method?.CreateDelegate(typeof(T));
    }
}