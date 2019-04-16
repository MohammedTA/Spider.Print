using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Spider.Print.Renderer
{
	public static class ReflectionExtensions
	{
		public static string GetEmbeddedResourceText(this Assembly assembly, string embeddedResourceName)
		{
			using (var stream = assembly.GetManifestResourceStream(embeddedResourceName))
			{
				if (stream == null)
				{
					var message = $"Embedded resource '{embeddedResourceName}' cannot be found in assembly '{assembly.FullName}'.";

					throw new ArgumentException(message);
				}

				using (var ms = new StreamReader(stream))
				{
					return ms.ReadToEnd();
				}
			}
		}
		public static IEnumerable<Type> GetBaseClasses(this Type type)
		{
			var current = type;
			while (current.BaseType != typeof(object) && current.BaseType != null)
			{
				yield return current.BaseType;
				current = current.BaseType;
			}
		}
		public static bool ImplementsGenericType(this Type me, Type genericType)
		{
			if (!genericType.IsGenericType)
			{
				throw new ArgumentException("Supplied argument is not a generic type", nameof(genericType));
			}

			return me.GetBaseClasses().Any(b => b.IsConstructedGenericType && b.GetGenericTypeDefinition() == genericType);
		}
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (var item in items)
			{
				action(item);
			}
		}

		public static Type GetBaseClassOfType(this Type type, Type baseClass)
		{
			if (type == baseClass)
			{
				return baseClass;
			}

			if (type.BaseType == null)
			{
				return null;
			}

			if (baseClass.IsGenericType)
			{
				// T1 : T2<int>
				if (type.BaseType.IsConstructedGenericType)
				{
					var genericType = baseClass.IsConstructedGenericType
						? type.BaseType
						: type.BaseType.GetGenericTypeDefinition();

					if (genericType == baseClass)
					{
						return type.BaseType.ContainsGenericParameters
							? type.BaseType.GetGenericTypeDefinition()
							: type.BaseType;
					}
				}
			}

			// T1 : T2
			return type.BaseType.GetBaseClassOfType(baseClass);
		}
		public static void RegisterUiMetadata(this DependencyInjectionContainer dependencyInjectionContainer, Assembly assembly)
		{
			dependencyInjectionContainer.GetInstance<PrintTemplateRegister>().RegisterAssembly(assembly);
		}
	}
}
