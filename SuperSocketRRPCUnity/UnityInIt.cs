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

        /// <summary>
        /// requestInfo container
        /// </summary>
        public PropertyInfo containerPropertyInfo { get; private set; }



        /// <summary>
        /// requestInfo container
        /// </summary>
        public PropertyInfo RequestClientSession { get; private set; }
       
        #endregion

        /// <summary>
        /// 容器对象 接口 FullName  实现类Type 实现类父对象是否为指定FullName
        /// </summary>
        private Dictionary<string,Tuple<Type,Type,bool>> ContainerObjList { get;set; }
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
        /// <param name="containerPropertyInfo">容器对象</param>
        /// <param name="requestClientSession">转发请求发送方SessionID</param>
        public UnityInIt(string BaseProvideServicesFullName, PropertyInfo socketPropertyInfo, PropertyInfo infoPropertyInfo, PropertyInfo requestInfoPropertyInfo, PropertyInfo containerPropertyInfo, PropertyInfo requestClientSession)
        {
            this.BaseProvideServicesFullName = BaseProvideServicesFullName;
            unityContainer = new UnityContainer();
            ContainerObjList = new Dictionary<string, Tuple<Type, Type, bool>>();

            this.socketPropertyInfo = socketPropertyInfo;
            this.infoPropertyInfo = infoPropertyInfo;
            this.requestInfoPropertyInfo = requestInfoPropertyInfo;
            this.containerPropertyInfo = containerPropertyInfo;
            this.RequestClientSession = requestClientSession;



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
                ContainerObjList.Add(typeIT.FullName, new Tuple<Type, Type, bool>(typeIT, typeT, true));
            }
            else {
                ContainerObjList.Add(typeIT.FullName, new Tuple<Type, Type, bool>(typeIT, typeT, false));
            }
          

            unityContainer.RegisterType<IT, T>();
        }
        /// <summary>
        /// 获取接口
        /// </summary>
        /// <param name="fullName">接口的FullName</param>
        /// <param name="obj">返回的对象</param>
        /// <param name="execType">对象类型</param>
        /// <param name="session">连接对象</param>
        /// <param name="info">请求信息</param>
        /// <param name="requestInfo">基础请求信息</param>
        /// <param name="container">容器对象</param>
        /// <param name="requestClientSession">转发对象</param>
        /// <returns></returns>
        public bool GetService(string fullName, Session session, Info info, RequestInfo requestInfo,IUnityContainer container,Guid? requestClientSession, out Object obj,out Type execType)
        {
            if (ContainerObjList.TryGetValue(fullName, out var value))
            {
                obj = unityContainer.Resolve(value.Item1);
                execType = value.Item2;
                if (value.Item3)
                {
                    //表示可以注入某些属性
                    socketPropertyInfo.SetValue(obj, session);
                    infoPropertyInfo.SetValue(obj, info);
                    requestInfoPropertyInfo.SetValue(obj, requestInfo);
                    containerPropertyInfo.SetValue(obj, container);
                    RequestClientSession.SetValue(obj, requestClientSession);
                }
                return true;
            }
            obj = null;
            execType = null;
            return false;
        }
    }
}
