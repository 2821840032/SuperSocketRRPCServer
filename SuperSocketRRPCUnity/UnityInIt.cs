using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace SuperSocketRRPCUnity
{
    public class UnityInIt
    {
        private Dictionary<string,Type> ContainerObjList { get;set; }
        public IUnityContainer unityContainer { get; set; }
        public UnityInIt()
        {
            unityContainer = new UnityContainer();
            ContainerObjList = new Dictionary<string, Type>();

        }
        /// <summary>
        /// 添加接口到容器中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddServer<IT,T>()
            where IT : class
            where T :IT
        {
            var typeIT = typeof(IT);
            if (ContainerObjList.TryGetValue(typeIT.FullName, out var value))
            {
                throw new Exception($"对象中已经有{typeIT.FullName}存在");
            }
            ContainerObjList.Add(typeIT.FullName, typeIT);

            unityContainer.RegisterType<IT, T>();
        }
        /// <summary>
        /// 获取接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            return unityContainer.Resolve<T>() ;
        }
        /// <summary>
        /// 获取接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Type GetService(string fullName,out Object obj)
        {
            if (ContainerObjList.TryGetValue(fullName, out var value))
            {
                obj = unityContainer.Resolve(value);
                return value;
            }
            throw new Exception($"没有找到{fullName} 键的存在");
        }

        /// <summary>
        /// 二进制 Copy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static T DeepCopyByReflection<T>(T source) where T:class
        {
            if (source == null)
                return null;
            Object objectReturn = null;
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, source);
                    stream.Position = 0;
                    objectReturn = formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return (T)objectReturn;
        }
        /// <summary>
        /// Copy unity
        /// </summary>
        /// <param name="unity"></param>
        /// <returns></returns>
        public static UnityInIt CopyTo(UnityInIt unity) {
            
            return DeepCopyByReflection(unity);
        }


    }
}
