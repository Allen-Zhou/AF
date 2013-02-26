using System;
using System.Collections;
using System.Collections.Generic;
//using NHibernate;
//using NHibernate.Classic;
//using System.Data.Objects.DataClasses;
using System.Reflection;
using TelChina.AF.Persistant.Exceptions;
using TelChina.AF.Sys.Context;
using System.Runtime.Serialization;
using System.Text;


namespace TelChina.AF.Persistant
{
    #region 委托
    /// <summary>
    /// 属性改变前委托
    /// </summary>
    /// <param name="name"></param>
    public delegate void PropertyChangIngDelegate(string propertyName);
    /// <summary>
    /// 属性改变后委托
    /// </summary>
    /// <param name="name"></param>
    public delegate void PropertyChangedDelegate(string propertyName);

    #endregion

    /// <summary>
    /// 领域实体需要需要实现的接口
    /// </summary>
    [DataContract]
    [Serializable]
    public abstract class EntityBase : IPersistableObject
    {
        #region 构造函数

        protected EntityBase()
        {
            this.ID = Guid.NewGuid();
        }

        #endregion

        #region 事件

        /// <summary>
        /// 属性改变前事件
        /// </summary>
        public virtual event PropertyChangIngDelegate OnPropertyChanging;

        /// <summary>
        /// 属性改变后事件
        /// </summary>
        public virtual event PropertyChangedDelegate OnPropertyChanged;

        /// <summary>
        /// 执行属性改变前事件
        /// </summary>
        /// <param name="PropertyName"></param>
        protected virtual void RaisPropertyChangIngEvent(string propertyName)
        {
            //只有新增状态才能修改ID
            //if (propertyName == "ID" && this.SysState != EntityStateEnum.Inserting)
            //{
            //    throw new Exception("只有新增状态才能修改ID");
            //}           

            if (OnPropertyChanging != null)
            {
                OnPropertyChanging(propertyName);
            }
        }

        /// <summary>
        /// 执行属性改变后事件
        /// </summary>
        /// <param name="PropertyName"></param>
        protected virtual void RaisPropertyChangedEvent(string propertyName)
        {
            //if (this.SysState == EntityStateEnum.Unchanged)
            //{
            //    this.SysState = EntityStateEnum.Updating;
            //}

            if (OnPropertyChanged != null)
            {
                OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// 集合属性改变事件
        /// 添加：张前园
        /// </summary>
        protected EventHandler<ItemChangedEventArgs<EntityBase>> SetItemChangedHandler;

        #endregion

        #region 基本属性

        /// <summary>
        /// 实体状态
        /// </summary>

        private EntityStateEnum sysState = EntityStateEnum.Unchanged;


        /// <summary>
        /// 实体的GUID
        /// </summary>
        private Guid id;

        /// <summary>
        /// 实体的创建时间
        /// </summary>
        private DateTime createdOn;

        /// <summary>
        /// 实体的创建人
        /// </summary>
        private string createdBy;

        /// <summary>
        /// 实体的最近一次跟新时间
        /// </summary>
        private DateTime updatedOn;

        /// <summary>
        /// 实体最近一次跟新人
        /// </summary>
        private string updatedBy;

        /// <summary>
        /// 版本号
        /// </summary>
        private Int32 sysVersion;

        /// <summary>
        /// 实体所在的组件名称
        /// </summary>
        public abstract string EntityComponent { get; set; }
        /// <summary>
        /// 实体的ID
        /// </summary>
        [DataMember(Order = 0)]
        [PropertyMetaDataAttribute(AllowNull = true)]
        public virtual Guid ID
        {
            get { return id; }
            set
            {
                if (this.ID != value)
                {
                    RaisPropertyChangIngEvent("ID");

                    id = value;

                    RaisPropertyChangedEvent("ID");
                }
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember(Order = 1)]
        public virtual DateTime CreatedOn
        {
            get { return createdOn; }
            set
            {
                if (this.createdOn != value)
                {

                    RaisPropertyChangIngEvent("CreatedOn");
                    createdOn = value;

                    RaisPropertyChangedEvent("createdOn");
                }
            }
        }

        ///<summary>
        /// 创建者
        ///</summary>
        [DataMember(Order = 2)]
        public virtual string CreatedBy
        {
            get { return createdBy; }
            set
            {
                if (this.createdBy != value)
                {
                    RaisPropertyChangIngEvent("CreatedBy");

                    createdBy = value;

                    RaisPropertyChangedEvent("CreatedBy");
                }
            }
        }



        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember(Order = 3)]
        public virtual DateTime UpdatedOn
        {
            get { return updatedOn; }
            set
            {
                if (this.updatedOn != value)
                {
                    RaisPropertyChangIngEvent("UpdatedOn");

                    updatedOn = value;

                    RaisPropertyChangedEvent("UpdatedOn");
                }
            }
        }

        /// <summary>
        /// 更新者
        /// </summary>
        [DataMember(Order = 4)]
        public virtual string UpdatedBy
        {
            get { return updatedBy; }
            set
            {
                if (this.updatedBy != value)
                {
                    RaisPropertyChangIngEvent("UpdatedBy");

                    updatedBy = value;

                    RaisPropertyChangedEvent("UpdatedBy");
                }
            }
        }

        /// <summary>
        /// 版本号,用于并发，开发人员不需要手工设置值
        /// 利用持久化层查询实体对象时，将会填充其值
        /// </summary>
        [DataMember(Order = 5)]
        public virtual Int32 SysVersion
        {
            get { return sysVersion; }
            set { sysVersion = value; }
        }

        /// <summary>
        /// 实体状态
        /// </summary>
        [DataMember(Order = 6)]
        public virtual EntityStateEnum SysState
        {
            get { return this.sysState; }
            set { this.sysState = value; }
        }


        #endregion

        #region 可扩展

        /// <summary>
        /// 设置本实体上的字段默认值,应用开发扩展点
        /// </summary>
        protected abstract void SetDefaultValue();

        /// <summary>
        /// 执行字段合法性检查,应用开发扩展点
        /// </summary>
        protected abstract void OnValidate();

        /// <summary>
        /// 新增保存的前置条件
        /// </summary>
        protected abstract void OnInserting();

        /// <summary>
        /// 新增保存的后置条件
        /// </summary>
        protected abstract void OnInserted();

        /// <summary>
        /// 修改保存的前置条件
        /// </summary>
        protected abstract void OnUpdating();

        /// <summary>
        /// 修改保存的后置条件
        /// </summary>
        protected abstract void OnUpdated();

        /// <summary>
        /// 删除的前置条件
        /// </summary>
        protected abstract void OnDeleting();

        /// <summary>
        /// 删除的后置条件
        /// </summary>
        protected abstract void OnDeleted();

        //protected abstract void ChangeSet(Iesi.Collections.Generic.ISet<EntityBase> iSet);

        protected virtual void ChangeSet(Iesi.Collections.Generic.ISet<EntityBase> iSet)
        {
        }


        #endregion

        #region 显示实现方法

        /// <summary>
        /// 设置实体属性默认值
        /// </summary>
        void IPersistableObject.SetDefaultValue()
        {

            if (this.SysState == EntityStateEnum.Inserting)
            {
                //ID
                if (ID == Guid.Empty)
                {
                    ID = Guid.NewGuid();
                    //关系关系中的ID
                }
                //CreatedOn = 会话中的登录信息登录时间;
                this.CreatedOn = DateTime.Now;
                //CreatedBy = 会话中的登录信息登录人;
                this.CreatedBy = ContextSession.Current.UserCode;
                //UpdatedBy = 会话中的登录信息登录人;
                //更新时间
                this.UpdatedOn = DateTime.Now;
                this.UpdatedBy = ContextSession.Current.UserCode;
            }
            //修改
            if (this.SysState == EntityStateEnum.Updating)
            {
                //UpdatedBy = 会话中的登录信息登录人;
                //更新时间
                this.UpdatedOn = DateTime.Now;
                this.UpdatedBy = ContextSession.Current.UserCode;
            }

            //ID
            if (ID == Guid.Empty)
            {
                //关系关系中的ID
                ID = Guid.NewGuid();
            }
            var minDate = new DateTime(1900, 1, 1);
            if (this.createdOn <= minDate)
            {
                //CreatedOn = 会话中的登录信息登录时间;
                this.CreatedOn = DateTime.Now;
            }
            if (this.updatedOn <= minDate)
            {
                //更新时间
                this.UpdatedOn = DateTime.Now;
            }
            if (string.IsNullOrEmpty(this.createdBy))
            {
                //CreatedBy = 会话中的登录信息登录人;
                this.CreatedBy = ContextSession.Current.UserCode;
            }
            if (string.IsNullOrEmpty(this.UpdatedBy))
            {
                //UpdatedBy = 会话中的登录信息登录人;
                this.UpdatedBy = ContextSession.Current.UserCode;
            }
            //业务设置字段默认值
            this.SetDefaultValue();
        }

        /// <summary>
        /// 实体效验
        /// </summary>
        void IPersistableObject.OnValidate()
        {
            //所有属性
            var allProperty = this.GetType().GetProperties();
            //空集合
            List<string> emptyList = new List<string>();
            //长度集合
            List<string> lengthList = new List<string>();
            foreach (PropertyInfo proInfo in allProperty)
            {
                //是否打上PropertyExpand标签
                var propertyExpand = proInfo.GetCustomAttributes(typeof(PropertyMetaDataAttribute), true);
                //存在PropertyExpand标签的属性
                if (propertyExpand.Length > 0)
                {
                    var expand = propertyExpand[0] as PropertyMetaDataAttribute;
                    //效验实体属性上打isNonEnpty=true的属性字段
                    var proInfovalue = proInfo.GetValue(this, null);
                    if (expand.AllowNull == false)
                    {
                        if (proInfovalue == null)
                        {
                            emptyList.Add(proInfo.Name);
                            //throw new Exception(EntityBaseResource.NonEmpty);
                        }
                    }
                    //效验实体属性上设置Length属性字段
                    if (expand.Length > 0)
                    {
                        if (proInfovalue != null && proInfovalue.ToString().Trim().Length > expand.Length)
                        {
                            lengthList.Add(string.Format(EntityBaseResource.OverLength, proInfo.Name, expand.Length));
                            //throw new Exception(EntityBaseResource.OverLength);
                        }
                    }
                }
            }
            if (emptyList.Count > 0)
            {
                string emptyException = string.Empty;
                foreach (string emptyProinfo in emptyList)
                {
                    emptyException += emptyProinfo + EntityBaseResource.NonEmpty + ";";
                }
                throw new ConcurrentModificationException(emptyException);
            }
            if (lengthList.Count > 0)
            {
                string lengthException = string.Empty;
                foreach (string lenProinfo in lengthList)
                {
                    lengthException += lenProinfo + EntityBaseResource.NonEmpty + ";";
                }
                throw new ConcurrentModificationException(lengthException);
            }
            //业务效验
            this.OnValidate();
        }

        /// <summary>
        /// 实体状态
        /// </summary>
        EntityStateEnum IPersistableObject.SysState
        {
            get { return this.SysState; }
            set { this.SysState = value; }
        }

        /// <summary>
        /// 新增保存的前置条件
        /// </summary>
        void IPersistableObject.OnInserting()
        {
            this.OnInserting();
        }

        /// <summary>
        /// 新增保存的后置条件
        /// </summary>
        void IPersistableObject.OnInserted()
        {
            this.SysState = EntityStateEnum.Unchanged;
            this.OnInserted();
        }

        /// <summary>
        /// 修改保存的前置条件
        /// </summary>
        void IPersistableObject.OnUpdating()
        {
            this.OnUpdating();
        }

        /// <summary>
        /// 修改保存的后置条件
        /// </summary>
        void IPersistableObject.OnUpdated()
        {
            this.SysState = EntityStateEnum.Unchanged;
            this.OnUpdated();
        }

        /// <summary>
        /// 删除的前置条件
        /// </summary>
        void IPersistableObject.OnDeleting()
        {
            this.OnDeleting();
        }

        /// <summary>
        /// 删除的后置条件
        /// </summary>
        void IPersistableObject.OnDeleted()
        {
            this.SysState = EntityStateEnum.Deleted;
            this.OnDeleted();
        }

        //protected virtual void ChangeSet(Iesi.Collections.Generic.ISet<EntityBase> iSet)
        //{
        //}
        #endregion


        #region 本版不需要
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(string.Format(",ID:{0},SysVersion:{1},SysState:{2}",
                this.ID, this.SysVersion, this.SysState));
            return result.ToString();
        }
        //public EntityBase()
        //{
        //    ////仓储中是否存在
        //    //var repository = RepositoryContext.CurrentRepository<EntityBase>();
        //    //if (repository != null)
        //    //{              
        //    //    this.SysState = EntityStateEnum.Inserting;
        //    //    repository.Add(this);
        //    //}
        //    //else
        //    //{
        //    //    this.SysState = EntityStateEnum.Detached;
        //    //}           
        //    this.SysState =EntityStateEnum.Inserting;
        //}

        //#region 方法
        //#region 删除实体
        ///// <summary>
        ///// 删除实体
        ///// </summary>
        //public virtual void Remove()
        //{
        //    //detached deleted状态不能做此操作
        //    if (this.SysState == EntityStateEnum.Detached || this.SysState == EntityStateEnum.Deleted)
        //    { 
        //        throw new Exception("Detached与Deleted不能做此操作！");
        //    }
        //    //Pl删除实体
        //    IRepository<EntityBase> repository = RepositoryContext.CurrentRepository<EntityBase>();
        //    if (repository != null)
        //    {
        //        repository.Remove(this);
        //    }
        //    RemoveSelfData();
        //}

        ///// <summary>
        ///// 删除默认值
        ///// </summary>
        //private void RemoveSelfData()
        //{
        //    //新增实体做删除操作时，直接不受管理
        //    if (this.SysState != EntityStateEnum.Inserting)
        //    {
        //        this.SysState = EntityStateEnum.Deleting;
        //    }
        //    else
        //    {
        //        this.SysState = EntityStateEnum.Detached;
        //    }
        //}
        //#endregion

        //#region 修改实体
        ///// <summary>
        ///// 修改实体
        ///// </summary>
        //protected virtual void Update()
        //{
        //    IRepository<EntityBase> repository = RepositoryContext.CurrentRepository<EntityBase>();
        //    //Pl修改实体
        //    if (repository != null)
        //    {
        //        repository.Update(this);
        //    }   

        //}
        ///// <summary>
        ///// 修改默认值
        ///// </summary>
        //private void UpdateSelfData()
        //{
        //    //只有状态为Unchanged状态才修改
        //    if (this.SysState == EntityStateEnum.Unchanged)
        //    {
        //        this.SysState = EntityStateEnum.Updating;
        //    }
        //}
        //#endregion

        //#region Detached 和Attach
        ///// <summary>
        ///// 设置实体为托管状态
        ///// </summary>
        ///// <param name="entityBase"></param>
        //public virtual void Detached()
        //{
        //    if (this.SysState == EntityStateEnum.Deleted)
        //    {
        //        throw new Exception("删除后的状态不能此操作！");
        //    }
        //    //通知PL实体为Detached状态
        //    IRepository<EntityBase> repository = RepositoryContext.CurrentRepository<EntityBase>();
        //    if (repository != null)
        //    {
        //        repository.Detach(this);
        //    }
        //    //设置状态为Detached
        //    this.SysState = EntityStateEnum.Detached;
        //}

        ///// <summary>
        ///// 设置实体为受管理状态
        ///// </summary>
        //public virtual void Attach()
        //{
        //    //不为Detached状态直接返回
        //    if(this.SysState!=EntityStateEnum.Deleted)
        //        return;
        //    //通知PL实体为NoChange状态
        //    IRepository<EntityBase> repository = RepositoryContext.CurrentRepository<EntityBase>();
        //    if (repository != null)
        //    {
        //        repository.Attach(this);
        //    }
        //    //设置状态为NoChange??新增状态Detach之后再UnDetach状态处理
        //    this.SysState = EntityStateEnum.Unchanged;
        //}

        //#endregion       
        //#region 属性改变
        ///// <summary>
        ///// 属性修改事件
        ///// </summary>
        ///// <param name="propertyName">改变的属性名称</param>
        //protected void OnPropertyChanging(string propertyName)
        //{

        //    ////只有新增状态才能修改ID
        //    //if (propertyName == "ID" && this.SysState != EntityStateEnum.Inserting)
        //    //{
        //    //    throw new Exception("只有新增状态才能修改ID");
        //    //}

        //    //if (this.SysState == EntityStateEnum.Unchanged)
        //    //{
        //    //    Update();
        //    //}
        //}

        ///// <summary>
        ///// 属性修改后事件
        ///// </summary>
        ///// <param name="propertyName">改变的属性名称</param>
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    //UpdateSelfData();
        //}
        //#endregion



        //#endregion 

        #endregion
    }





}
