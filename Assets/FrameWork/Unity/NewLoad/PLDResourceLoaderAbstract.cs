﻿using System;
using System.Collections.Generic;

namespace Pld
{
    abstract class PLDResourceLoaderAbstract : PLDRefObject, IPLDDisposeObject
    {
        #region 定义委托

        /// <summary>
        /// 开始委托
        /// </summary>
        public delegate void StartDelgate();

        /// <summary>
        /// 载入过程委托
        /// </summary>
        /// <param name="process"></param>
        public delegate void ProcessDelgate(float process);

        /// <summary>
        /// 成功委托
        /// </summary>
        /// <param name="resultObject">载入得到的对象</param>
        public delegate void SuccessDelgate(object resultObject);

        /// <summary>
        /// 失败委托
        /// </summary>
        /// <param name="msg">失败的信息</param>
        public delegate void ErrorDelgate(string msg);

        /// <summary>
        /// 完成委托
        /// </summary>
        /// <param name="isOk">是否成功</param>
        /// <param name="resultObject">载入得到的对象</param>
        public delegate void FinishDelgate(bool isOk, object resultObject);

        #endregion

        #region 成员对象

        /// <summary>
        /// 最终加载的结果
        /// </summary>
        public object ResultObj { get; private set; }

        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool IsError { get; private set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinish { get; private set; }

        /// <summary>
        /// 是否准备Dispose
        /// </summary>
        public bool IsReadyDispose { get; private set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 进度
        /// </summary>
        public float Process { get; private set; }

        /// <summary>
        /// 开始回调
        /// </summary>
        public StartDelgate mStartCallback { private get; set; }

        /// <summary>
        /// 进行回调
        /// </summary>
        public ProcessDelgate mProcessCallback { private get; set; }

        /// <summary>
        /// 成功回调
        /// </summary>
        public SuccessDelgate mSuccessCallback { private get; set; }

        /// <summary>
        /// 失败回调
        /// </summary>
        public ErrorDelgate mErrorCallbcak { private get; set; }

        /// <summary>
        /// 完成回调
        /// </summary>
        public FinishDelgate mFinishCallback { private get; set; }

        #endregion

        public PLDResourceLoaderAbstract()
        {
            
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init(string url)
        {
            ResultObj = null;
            IsReadyDispose = false;

            IsError = false;
            IsSuccess = false;
            IsFinish = false;

            Url = url;
            Message = "";

            Process = 0.0F;

            mStartCallback = null;
            mProcessCallback = null;
            mSuccessCallback = null;
            mErrorCallbcak = null;
            mFinishCallback = null;
        }

        /// <summary>
        /// 从准备Dispose中恢复
        /// </summary>
        public virtual void Revice()
        {
            IsReadyDispose = false;
        }

        /// <summary>
        /// 准备Dispose回调用
        /// </summary>
        public virtual void OnReadyDispose()
        {
            IsReadyDispose = true;
        }

        /// <summary>
        /// 释放对象，子类一般重写DoDispose
        /// </summary>
        public void Dispose()
        {
            DoDispose();
        }

        /// <summary>
        /// 释放对象时进行的操作
        /// </summary>
        public virtual void DoDispose()
        {

        }

        /// <summary>
        /// 开始
        /// </summary>
        public virtual void OnStart()
        {
            Message = "Start";
            Process = 0.0F;

            if (mStartCallback != null)
                mStartCallback();
        }

        /// <summary>
        /// 进度变化
        /// </summary>
        /// <param name="process"></param>
        public virtual void OnProcess(float process)
        {
            Message = "Process";
            Process = process;

            if (mProcessCallback != null)
                mProcessCallback(Process);
        }

        /// <summary>
        /// 成功
        /// </summary>
        public virtual void OnSuccess()
        {
            Message = "Success";
            if (mSuccessCallback != null)
                mSuccessCallback(ResultObj);
        }

        /// <summary>
        /// 出错
        /// </summary>
        public virtual void OnError(string msg = "")
        {
            Message = msg == "" ? "Error" : msg;
            if (mErrorCallbcak != null)
                mErrorCallbcak(Message);
        }

        /// <summary>
        /// 加载完成
        /// </summary>
        /// <param name="resultObj"></param>
        public virtual void OnFinish(object resultObj)
        {
            ResultObj = resultObj;
            Process = 100.0F;

            IsError = resultObj == null;
            IsSuccess = !IsError;

            if (IsSuccess)
                OnSuccess();

            if (IsError)
                OnError();

            IsFinish = true;
            if(mFinishCallback != null)
            {
                mFinishCallback(IsSuccess, ResultObj);
            }
        }
    }

    
}