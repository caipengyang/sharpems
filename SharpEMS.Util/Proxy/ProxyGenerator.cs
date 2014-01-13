using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace SharpEMS.Util.Proxy
{
	public class ProxyGenerator<T> : Singleton<ProxyGenerator<T> >
	{
		public T ProxyObject{ get; set; }

		public ProxyGenerator()
		{
		}

		public void SetProxyGenerator(IProxyHandler proxyHandler)
		{
			AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly (new AssemblyName ("SharpEMS.Client.Proxy"), AssemblyBuilderAccess.Run);
			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule (assemblyBuilder.GetName ().Name);
			Type interfaceType = typeof(T);
			TypeBuilder typeBuilder = moduleBuilder.DefineType (string.Format ("{0}_Proxy", interfaceType.Name), TypeAttributes.Class | TypeAttributes.Public, typeof(Object), new Type[]{interfaceType});
			ConstructorInfo objectConstructor = typeof(Object).GetConstructor (new Type[]{});
			FieldBuilder proxyFieldBuilder = typeBuilder.DefineField ("handler", typeof(IProxyHandler), FieldAttributes.Private);
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor (MethodAttributes.Public, CallingConventions.HasThis, new Type[]{typeof(IProxyHandler)});
			ILGenerator ilGenerator = constructorBuilder.GetILGenerator ();
			ilGenerator.Emit (OpCodes.Ldarg_0);
			ilGenerator.Emit (OpCodes.Call, objectConstructor);
			ilGenerator.Emit (OpCodes.Ldarg_0);
			ilGenerator.Emit (OpCodes.Ldarg_1);
			ilGenerator.Emit (OpCodes.Stfld, proxyFieldBuilder);
			ilGenerator.Emit (OpCodes.Ret);

			MethodInfo proxyMethodInfo = proxyHandler.GetType ().GetMethod ("Invoke", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] {
				//typeof(Object),
				typeof(MethodInfo),
				typeof(object[])
			},
				null);
			foreach (MethodInfo interfaceMethodInfo in interfaceType.GetMethods()) {
				MethodBuilder methodBuilder = typeBuilder.DefineMethod (interfaceMethodInfo.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, interfaceMethodInfo.ReturnType, interfaceMethodInfo.GetParameters ().Select (p => p.ParameterType).ToArray ());
				ILGenerator il = methodBuilder.GetILGenerator ();
				int privateParameterCouunt = interfaceMethodInfo.GetParameters ().Length;
				LocalBuilder argArray = il.DeclareLocal (typeof(object[]));
				il.Emit (OpCodes.Ldc_I4, privateParameterCouunt);
				il.Emit (OpCodes.Newarr, typeof(object));
				il.Emit (OpCodes.Stloc, argArray);
				LocalBuilder methodInfo = il.DeclareLocal (typeof(MethodInfo));
				il.Emit (OpCodes.Ldtoken, interfaceMethodInfo);
				il.Emit (OpCodes.Call, typeof(MethodBase).GetMethod ("GetMethodFromHandle", new Type[] { typeof(RuntimeMethodHandle) }));
				il.Emit (OpCodes.Stloc, methodInfo);

				for (int i =0; i < interfaceMethodInfo.GetParameters().Length; ++i) {
					ParameterInfo info = interfaceMethodInfo.GetParameters () [i];
					il.Emit (OpCodes.Ldloc, argArray);
					il.Emit (OpCodes.Ldc_I4, i);
					il.Emit (OpCodes.Ldarg_S, i + 1);
					if (info.ParameterType.IsPrimitive || info.ParameterType.IsValueType)
						il.Emit (OpCodes.Box, info.ParameterType);
					il.Emit (OpCodes.Stelem_Ref);
				}
				il.Emit (OpCodes.Ldarg_0);
				il.Emit (OpCodes.Ldfld, proxyFieldBuilder);
				il.Emit (OpCodes.Ldloc, methodInfo);
				il.Emit (OpCodes.Ldloc, argArray);
				il.Emit (OpCodes.Call, proxyMethodInfo);
				if (interfaceMethodInfo.ReturnType.IsValueType && interfaceMethodInfo.ReturnType != typeof(void))
					il.Emit (OpCodes.Unbox_Any, interfaceMethodInfo.ReturnType);
				if (interfaceMethodInfo.ReturnType == typeof(void))
					il.Emit (OpCodes.Pop);
				il.Emit (OpCodes.Ret);

			}
			this.ProxyObject = (T)Activator.CreateInstance (typeBuilder.CreateType (), new Object[]{proxyHandler});
		}

		private ProxyGenerator (IProxyHandler proxyHandler)
		{
			SetProxyGenerator (proxyHandler);
		}

		public static T getProxyObject()
		{
			return Instance ().ProxyObject;
		}
	}
}

