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
    /// <summary>
    /// 初始化一个容器
    /// </summary>
    public class UnityInIt
    {

        /// <summary>
        ///  BaseProvideServices FulleName 
        /// </summary>
        string BaseProvideServicesFullName { get; set; }

        /// <summary>
        /// 容器对象 接口 FullName  实现类Type 实现类父对象是否为指定FullName
        /// </summary>
        private Dictionary<string,Tuple<Type,bool>> ContainerObjList { get;set; }
        /// <summary>
        /// unity 容器对象对象
        /// </summary>
        public IUnityContainer unityContainer { get; set; }
        /// <summary>
        /// 初始化容器对象
        /// </summary>
        /// <param name="BaseProvideServicesFullName">需要监听的对象值</param>
        public UnityInIt(string BaseProvideServicesFullName)
        {
            this.BaseProvideServicesFullName = BaseProvideServicesFullName;
            unityContainer = new UnityContainer();
            ContainerObjList = new Dictionary<string, Tuple<Type, bool>>();

        }
        /// <summary>
        /// 添加接口到容器中
        /// </summary>
        /// <typeparam name="IT">接口</typeparam>
        /// <typeparam name="T">实现</typeparam>
        public void AddServer<IT,T>()
            where IT : class
            where T :IT
        {
            var typeIT = typeof(IT);
            var typeT = typeof(T);
          
            if (ContainerObjList.TryGetValue(typeIT.FullName, out var value))
            {
                throw new Exception($"对象中已经有{typeIT.FullName}存在");
            }
            if (typeT.BaseType != null && typeT.BaseType.FullName.Equals(BaseProvideServicesFullName))
            {
                ContainerObjList.Add(typeIT.FullName, new Tuple<Type, bool>(typeIT, true));
            }
            else {
                ContainerObjList.Add(typeIT.FullName, new Tuple<Type, bool>(typeIT, false));
            }
          

            unityContainer.RegisterType<IT, T>();
        }
        /// <summary>
        /// 获取接口
        /// </summary>
        /// <param name="fullName">接口的FullName</param>
        /// <param name="obj">返回的对象</param>
        /// <param name="InterfaceType">接口类型</param>
        /// <param name="isMatchingFullName">实现对象是否为FullName</param>
        /// <returns></returns>
        public bool GetService(string fullName,out Object obj,out Type InterfaceType,out bool isMatchingFullName)
        {
            if (ContainerObjList.TryGetValue(fullName, out var value))
            {
                obj = unityContainer.Resolve(value.Item1);
                InterfaceType = value.Item1;
                isMatchingFullName = value.Item2;
                return true;
            }
            obj = null;
            InterfaceType = null;
            isMatchingFullName = false;
            return false;
        }
    }
}
