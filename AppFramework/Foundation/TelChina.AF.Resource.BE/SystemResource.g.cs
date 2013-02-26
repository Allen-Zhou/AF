using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Resource
{
    public partial class SystemResource : EntityBase
    {
        /// <summary>
        /// 类名称
        /// </summary>
        private string _resourceType;

        /// <summary>
        /// 列名称
        /// </summary>
        private string _resourceCode;

        /// <summary>
        /// 列描述
        /// </summary>
        private string _resourceName;

        /// <summary>
        /// 是否可见
        /// </summary>
        private bool _isVisible;

        /// <summary>
        /// 排序号
        /// </summary>
        private string _orderNo;

        /// <summary>
        /// 类名称
        /// </summary>
        public virtual string ResourceType
        {
            get
            {
                return this._resourceType;
            }
            set
            {
                if (this._resourceType != value)
                {
                    RaisPropertyChangIngEvent("ResourceType");
                    _resourceType = value;
                    RaisPropertyChangedEvent("ResourceType");
                }
            }
        }

        /// <summary>
        /// 列名称
        /// </summary>
        public virtual string ResourceCode
        {
            get
            {
                return this._resourceCode;
            }
            set
            {
                if (this._resourceCode != value)
                {
                    RaisPropertyChangIngEvent("ResourceCode");
                    _resourceCode = value;
                    RaisPropertyChangedEvent("ResourceCode");
                }
            }
        }

        /// <summary>
        /// 列描述
        /// </summary>
        public virtual string ResourceName
        {
            get
            {
                return this._resourceName;
            }
            set
            {
                if (this._resourceName != value)
                {
                    RaisPropertyChangIngEvent("ResourceName");
                    _resourceName = value;
                    RaisPropertyChangedEvent("ResourceName");
                }
            }
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        public virtual bool IsVisible
        {
            get
            {
                return this._isVisible;
            }
            set
            {
                if (this._isVisible != value)
                {
                    RaisPropertyChangIngEvent("IsVisible");
                    _isVisible = value;
                    RaisPropertyChangedEvent("IsVisible");
                }
            }
        }

        /// <summary>
        /// 排序号
        /// </summary>
        public virtual string OrderNo
        {
            get
            {
                return this._orderNo;
            }
            set
            {
                if (this._orderNo != value)
                {
                    RaisPropertyChangIngEvent("OrderNo");
                    _orderNo = value;
                    RaisPropertyChangedEvent("OrderNo");
                }
            }
        }

        private string entityComponet;

        public override string EntityComponent
        {
            get
            {
                return entityComponet;
            }
            set
            {
                this.entityComponet = value;
            }
        }
    }
}
