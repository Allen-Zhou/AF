using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;


namespace TelChina.AF.Persistant.NHImpl
{
    /// <summary>
    /// 实体事件监听器，能够监听到实体CRUD的前后事件，可用于扩展
    /// </summary>
    public class EntityListener :   IPostLoadEventListener, IPostUpdateEventListener, IPostInsertEventListener,  IPostDeleteEventListener
    {
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            var persistableObject = @event.Entity as IPersistableObject;           
            persistableObject.OnUpdated();
        }

       

        public void OnPostLoad(PostLoadEvent @event)
        {
            var persistableObject = @event.Entity as IPersistableObject;
            if (persistableObject != null)
            {
                persistableObject.SysState = EntityStateEnum.Unchanged;
            }
        }

    

        public void OnPostInsert(PostInsertEvent @event)
        {
            var persistableObject = @event.Entity as IPersistableObject;            
            persistableObject.OnInserted();
            //if (persistableObject != null)
            //{
            //    persistableObject.SysState = EntityStateEnum.Unchanged;
            //}
        }

      /*  public bool OnPreDelete(PreDeleteEvent @event)
        {
            return false;
        }*/

        public void OnPostDelete(PostDeleteEvent @event)
        {
            var persistableObject = @event.Entity as IPersistableObject;
            persistableObject.OnDeleted();
            //if (persistableObject != null)
            //{
            //    persistableObject.SysState = EntityStateEnum.Deleted;
            //}
        }
    }
}
