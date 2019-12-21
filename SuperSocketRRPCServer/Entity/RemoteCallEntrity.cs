﻿using SuperSocketRRPCServer.CommunicationEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SuperSocketRRPCServer.Entity
{
    /// <summary>
    /// 任务存储的信息
    /// </summary>
    public class RemoteCallEntrity
    {

        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public ReceiveMessageState State { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// 远程任务处理结果
        /// </summary>
        public string ReturnValue { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        public RequestExecutiveInformation TaskInfo { get; set; }

        /// <summary>
        /// 通讯对象
        /// </summary>
        public MySession ClientSocket { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; } = 0;

        /// <summary>
        /// 通知执行委托
        /// </summary>
        public EventWaitHandle WaitHandle { get; set; }

        public RemoteCallEntrity(Guid id, RequestExecutiveInformation taskInfo, ReceiveMessageState state, DateTime expirationTime, MySession socket) {
            this.ID = id;
            this.TaskInfo = taskInfo;
            this.State = state;
            this.ExpirationTime = expirationTime;
            this.ClientSocket = socket;

            WaitHandle = new AutoResetEvent(false);
        }
        /// <summary>
        /// 修改结果并执行委托函数
        /// </summary>
        /// <param name="rpcResult"></param>
        public void ProcessingFuncInvoke(string rpcResult) {
            ReturnValue = rpcResult;
            WaitHandle.Set();

        }

        /// <summary>
        /// 修改结果并执行委托函数
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="rpcResult">结果</param>
        public void ProcessingFuncInvoke(ReceiveMessageState state,string rpcResult)
        {
            State = state;
            ReturnValue = rpcResult;
            WaitHandle.Set();

        }
    }
}
