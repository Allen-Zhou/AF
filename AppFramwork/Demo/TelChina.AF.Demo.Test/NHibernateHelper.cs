using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;

namespace TelChina.AF.Demo.Test
{
    public class NHibernateHelper
    {
        private static readonly ISessionFactory sessionFactory;

        static NHibernateHelper()
        {
            sessionFactory = new Configuration().Configure().BuildSessionFactory();

            //Configuration cfg = new Configuration()
            //.AddAssembly("Cat.hbm.xml");
            //sessionFactory = cfg.BuildSessionFactory();
        }

        public static IDbCommand GetIDbCommand()
        {
            return ((ISessionFactoryImplementor)sessionFactory).ConnectionProvider.Driver.CreateConnection().CreateCommand();
        }

        public ISession GetCurrentSession()
        {
            ISession currentSession = sessionFactory.OpenSession();
            return currentSession;
        }

        public static void CloseSession()
        {
            sessionFactory.Close();
        }

        public static void CloseSessionFactory()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close();
            }
        }
    }
}
