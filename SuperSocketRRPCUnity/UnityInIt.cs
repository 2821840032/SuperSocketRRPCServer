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
    public class UnityInIt<Session,Info, RequestInfo>
    {

        #region 反射设置值的对象列表

        /// <summary>
        ///  BaseProvideServices FulleName 
        /// </summary>
        public string BaseProvideServicesFullName { get; private set; }

        /// <summary>
        /// Socket PropertyInfo
        /// </summary>
        public PropertyInfo socketPropertyInfo { get; private set; }

        /// <summary>
        /// info PropertyInfo
        /// </summary>
        public PropertyInfo infoPropertyInfo { get; private set; }

        /// <summary>
        /// requestInfo PropertyInfo
        /// </summary>
        public PropertyInfo requestInfoPropertyInfo { get; private set; }

        #endregion

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
        /// <param name="socketPropertyInfo">连接对象属性Type</param>
        /// <param name="infoPropertyInfo">请求信息对象属性Type</param>
        /// <param name="requestInfoPropertyInfo">基础请求信息对象属性</param>
        public UnityInIt(string BaseProvideServicesFullName, PropertyInfo socketPropertyInfo, PropertyInfo infoPropertyInfo, PropertyInfo requestInfoPropertyInfo)
        {
            this.BaseProvideServicesFullName = BaseProvideServicesFullName;
            unityContainer = new UnityContainer();
            ContainerObjList = new Dictionary<string, Tuple<Type, bool>>();

            this.socketPropertyInfo = socketPropertyInfo;
            this.infoPropertyInfo = infoPropertyInfo;
            this.requestInfoPropertyInfo = requestInfoPropertyInfo;



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
        /// <param name="session">连接对象</param>
        /// <param name="info">请求信息</param>
        /// <param name="requestInfo">基础请求信息</param>
        /// <returns></returns>
        public bool GetService(string fullName, Session session, Info info, RequestInfo requestInfo, out Object obj,out Type InterfaceType)
        {
            if (ContainerObjList.TryGetValue(fullName, out var value))
            {
                obj = unityContainer.Resolve(value.Item1);
                InterfaceType = value.Item1;
                if (value.Item2)
                {
                    //表示可以注入某些属性
                    socketPropertyInfo.SetValue(obj, session);
                    infoPropertyInfo.SetValue(obj, info);
                   requestInfoPropertyInfo.SetValue(obj, requestInfo);
                }
                return true;
            }
            obj = null;
            InterfaceType = null;
            return false;
        }
    }
}
