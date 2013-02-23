using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public partial class Department:EntityBase
    {
        #region 属性

        /// <summary>
        /// 编号
        /// </summary>
        private string code;

        /// <summary>
        /// 编号
        /// </summary>
        public virtual string Code
        {
            get { return this.code; }
            set
            {
                if ((this.Code != null && this.code.Trim() != value) || (this.Code == null && value != null))
                {
                    RaisPropertyChangIngEvent("Code");
                    code = value;
                    RaisPropertyChangedEvent("Code");
                }
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        private string name;

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    RaisPropertyChangIngEvent("Name");
                    name = value;
                    RaisPropertyChangedEvent("Name");
                }
            }
        }
        /// <summary>
        /// 停用标志
        /// </summary>
        private bool disabled;

        /// <summary>
        /// 是否末级
        /// </summary>
        private bool isEndNode;

        /// <summary>
        /// 级次
        /// </summary>
        private int depth;

        /// <summary>
        /// 级次ID
        /// </summary>
        private string inId;

        /// <summary>
        /// 部门
        /// </summary>
        private Department parent;

        /// <summary>
        /// 部门
        /// </summary>
        public virtual Department Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (this.parent != value)
                {
                    //RaisPropertyChangIngEvent("Parent");
                    parent = value;
                    //RaisPropertyChangedEvent("Parent");
                }

            }
        }

        /// <summary>
        /// 停用
        /// </summary>
        public virtual bool Disabled
        {
            get
            {
                return disabled;
            }
            set
            {
                if (this.disabled != value)
                {
                    RaisPropertyChangIngEvent("Disabled");
                    disabled = value;
                    RaisPropertyChangedEvent("Disabled");
                }

            }
        }

        /// <summary>
        /// 是否末级节点
        /// </summary>
        public virtual bool IsEndNode
        {
            get
            {
                return isEndNode;
            }
            set
            {
                if (this.isEndNode != value)
                {
                    RaisPropertyChangIngEvent("IsEndNode");
                    isEndNode = value;
                    RaisPropertyChangedEvent("IsEndNode");
                }

            }
        }

        /// <summary>
        /// 深度
        /// </summary>
        public virtual int Depth
        {
            get
            {
                return depth;
            }
            set
            {
                if (this.depth != value)
                {
                    RaisPropertyChangIngEvent("Depth");
                    depth = value;
                    RaisPropertyChangedEvent("Depth");
                }

            }
        }

        /// <summary>
        /// 级次ID
        /// </summary>
        public virtual string InId
        {
            get
            {
                return inId;
            }
            set
            {
                if (this.inId != value)
                {
                    RaisPropertyChangIngEvent("InId");
                    inId = value;
                    RaisPropertyChangedEvent("InId");
                }
            }
        }

        public virtual void AddChild(Person child)
        {
            this.Persons.Add(child);
            child.Department = this;
        }

        public virtual void RemoveChild(Person child)
        {
            this.Persons.Remove(child);
            child.Department = null;
        }

        private Guid _idParent;

        /// <summary>
        /// 所属上级部门ID
        /// </summary>
        public virtual Guid idParent
        {
            get { return _idParent; }
            set { _idParent = value; }
        }

        public override string EntityComponent
        {
            get;
            set;
        }        
        #endregion

        #region 部门员工
        private IObservableSet<Person> _persons = new ObservableSet<Person>() ;

        /// <summary>
        /// 部门中的人员
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Person> Persons
        {
            get
            {
                return _persons;
            }
            set
            {
                this.ChangePersonSet(value);
            }
        }

        //Hashtable ht = new Hashtable();
        //List<EventHandler<ItemChangedEventArgs<Person>>>  list = new List<EventHandler<ItemChangedEventArgs<Person>>>();         
        private EventHandler<ItemChangedEventArgs<Person>> _personSetItemChangedHandler;        

        private void ChangePersonSet(Iesi.Collections.Generic.ISet<Person> persons)
        {
            if (persons == null)
            {
                return;
            }
            if (this._personSetItemChangedHandler == null)
            {
                this._personSetItemChangedHandler = this.OnPersonSetItemChanged;
            }

            if (this._persons != null)
            {
                this._persons.ItemChanged -= this._personSetItemChangedHandler;
            }

            foreach (var person in persons)
            {
                this._persons.Add(person);
            }

            this._persons.ItemChanged += this._personSetItemChangedHandler;
        }

        private void OnPersonSetItemChanged(object sender, ItemChangedEventArgs<Person> e)
        {
            var person = e.Item;

            if (e.Type == ItemChangedType.Added)
            {
                if (person.Department != null)
                {
                    person.Department.Persons.Remove(person);
                }

                person.Department = this;
            }
            else
            {
                person.Department = null;
            }
        }
        #endregion

        #region 下级部门

        private IObservableSet<Department> _childrenDepartment = new ObservableSet<Department>();

        /// <summary>
        /// 部门的下级部门
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Department> ChildrenDepartment
        {
            get
            {
                return _childrenDepartment;
            }
            set
            {
                this.ChangePersonSet(value);
            }
        }

        private EventHandler<ItemChangedEventArgs<Department>> __childrenDepartmentSetItemChangedHandler;

        private void ChangePersonSet(Iesi.Collections.Generic.ISet<Department> childrenDepartment)
        {
            if (childrenDepartment == null)
            {
                return;
            }
            if (this.__childrenDepartmentSetItemChangedHandler == null)
            {
                this.__childrenDepartmentSetItemChangedHandler = this.OnChildrenSetItemChanged;
            }

            if (this._childrenDepartment != null)
            {
                this._childrenDepartment.ItemChanged -= this.__childrenDepartmentSetItemChangedHandler;
            }

            foreach (var childDepartment in childrenDepartment)
            {
                this._childrenDepartment.Add(childDepartment);
            }

            this._childrenDepartment.ItemChanged += this.OnChildrenSetItemChanged;
        }

        private void OnChildrenSetItemChanged(object sender, ItemChangedEventArgs<Department> e)
        {
            var childDepartment = e.Item;

            if (e.Type == ItemChangedType.Added)
            {
                if (childDepartment.Parent != null)
                {
                    childDepartment.Parent.ChildrenDepartment.Remove(childDepartment);
                }

                childDepartment.Parent = this;
            }
            else
            {
                childDepartment.Parent = null;
            }
        }
        #endregion 
    }
}
